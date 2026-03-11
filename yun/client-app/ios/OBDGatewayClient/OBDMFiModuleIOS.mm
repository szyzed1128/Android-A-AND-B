#import "OBDMFiModuleIOS.h"

#import <ExternalAccessory/ExternalAccessory.h>
#import <React/RCTLog.h>

static NSString *const kOBDMFiDataEvent = @"OBDMFiDataReceived";
static NSString *const kOBDMFiClosedEvent = @"OBDMFiSessionClosed";
static NSString *const kOBDMFiAccessoryConnectedEvent = @"OBDMFiAccessoryConnected";
static NSString *const kOBDMFiAccessoryDisconnectedEvent = @"OBDMFiAccessoryDisconnected";
static NSTimeInterval const kOBDMFiWriteTimeoutSeconds = 2.0;
static NSUInteger const kOBDMFiLogPreviewBytes = 96;

@interface OBDMFiModuleIOS ()

@property (nonatomic, assign) BOOL hasListeners;
@property (nonatomic, strong) NSMutableDictionary<NSString *, EASession *> *sessions;
@property (nonatomic, strong) NSMapTable<NSInputStream *, NSString *> *inputStreamSessionIds;
@property (nonatomic, strong) NSMapTable<NSOutputStream *, NSString *> *outputStreamSessionIds;
@property (nonatomic, strong) NSMutableDictionary<NSString *, EAAccessory *> *sessionAccessories;
@property (nonatomic, strong) NSMutableDictionary<NSString *, NSNumber *> *sessionTxSequence;
@property (nonatomic, strong) NSMutableDictionary<NSString *, NSNumber *> *sessionRxSequence;

@end

@implementation OBDMFiModuleIOS

RCT_EXPORT_MODULE(OBDMFiModuleIOS)

+ (BOOL)requiresMainQueueSetup
{
  return YES;
}

- (void)logInfo:(NSString *)stage message:(NSString *)message extra:(NSDictionary *)extra
{
  RCTLogInfo(@"[MFiNative][%@] %@ %@", stage ?: @"unknown", message ?: @"", extra ?: @{});
}

- (void)logWarn:(NSString *)stage message:(NSString *)message extra:(NSDictionary *)extra
{
  RCTLogWarn(@"[MFiNative][%@] %@ %@", stage ?: @"unknown", message ?: @"", extra ?: @{});
}

- (NSUInteger)nextSequenceForSessionId:(NSString *)sessionId
                                 store:(NSMutableDictionary<NSString *, NSNumber *> *)store
{
  if (sessionId.length == 0 || !store) {
    return 0;
  }

  NSUInteger next = store[sessionId].unsignedIntegerValue + 1;
  store[sessionId] = @(next);
  return next;
}

- (BOOL)data:(NSData *)data containsByte:(uint8_t)target
{
  if (!data || data.length == 0) {
    return NO;
  }

  const uint8_t *bytes = (const uint8_t *)data.bytes;
  for (NSUInteger i = 0; i < data.length; i++) {
    if (bytes[i] == target) {
      return YES;
    }
  }
  return NO;
}

- (NSString *)previewForData:(NSData *)data
{
  if (!data || data.length == 0) {
    return @"";
  }

  NSString *decoded = [[NSString alloc] initWithData:data encoding:NSISOLatin1StringEncoding];
  if (!decoded) {
    decoded = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
  }
  if (!decoded) {
    NSMutableString *hex = [NSMutableString string];
    const uint8_t *bytes = (const uint8_t *)data.bytes;
    NSUInteger limit = MIN((NSUInteger)16, data.length);
    for (NSUInteger i = 0; i < limit; i++) {
      [hex appendFormat:@"%02X", bytes[i]];
      if (i + 1 < limit) {
        [hex appendString:@" "];
      }
    }
    if (data.length > limit) {
      [hex appendString:@" ..."];
    }
    return [NSString stringWithFormat:@"[hex %@]", hex];
  }

  NSString *sanitized = [[decoded stringByReplacingOccurrencesOfString:@"\r" withString:@"\\r"]
      stringByReplacingOccurrencesOfString:@"\n" withString:@"\\n"];
  if (sanitized.length > kOBDMFiLogPreviewBytes) {
    return [[sanitized substringToIndex:kOBDMFiLogPreviewBytes] stringByAppendingString:@"..."];
  }
  return sanitized;
}

- (NSDictionary *)debugInfoForData:(NSData *)data
                         sessionId:(NSString *)sessionId
                          sequence:(NSUInteger)sequence
{
  return @{
    @"sessionId": sessionId ?: @"",
    @"seq": @(sequence),
    @"bytes": @(data.length),
    @"hasCR": @([self data:data containsByte:'\r']),
    @"hasLF": @([self data:data containsByte:'\n']),
    @"hasPrompt": @([self data:data containsByte:'>']),
    @"preview": [self previewForData:data]
  };
}

- (instancetype)init
{
  self = [super init];
  if (self) {
    _hasListeners = NO;
    _sessions = [NSMutableDictionary new];
    _inputStreamSessionIds = [NSMapTable weakToStrongObjectsMapTable];
    _outputStreamSessionIds = [NSMapTable weakToStrongObjectsMapTable];
    _sessionAccessories = [NSMutableDictionary new];

    [[EAAccessoryManager sharedAccessoryManager] registerForLocalNotifications];
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(onAccessoryDidConnect:)
                                                 name:EAAccessoryDidConnectNotification
                                               object:nil];
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(onAccessoryDidDisconnect:)
                                                 name:EAAccessoryDidDisconnectNotification
                                               object:nil];
    [self logInfo:@"lifecycle" message:@"module init" extra:nil];
  }
  return self;
}

- (void)dealloc
{
  [self logInfo:@"lifecycle" message:@"module dealloc" extra:nil];
  [[NSNotificationCenter defaultCenter] removeObserver:self];
  [[EAAccessoryManager sharedAccessoryManager] unregisterForLocalNotifications];
  [self closeAllSessionsWithReason:@"module_dealloc"];
}

- (void)startObserving
{
  self.hasListeners = YES;
  [self logInfo:@"listener" message:@"start observing" extra:nil];
}

- (void)stopObserving
{
  self.hasListeners = NO;
  [self logInfo:@"listener" message:@"stop observing" extra:nil];
}

- (NSArray<NSString *> *)supportedEvents
{
  return @[
    kOBDMFiDataEvent,
    kOBDMFiClosedEvent,
    kOBDMFiAccessoryConnectedEvent,
    kOBDMFiAccessoryDisconnectedEvent
  ];
}

RCT_REMAP_METHOD(isSupported,
                 isSupportedWithResolver:(RCTPromiseResolveBlock)resolve
                 rejecter:(RCTPromiseRejectBlock)reject)
{
  (void)reject;
  BOOL supported = (NSClassFromString(@"EAAccessoryManager") != nil);
  [self logInfo:@"capability" message:@"isSupported checked" extra:@{ @"supported": @(supported) }];
  resolve(@(supported));
}

RCT_REMAP_METHOD(getConnectedAccessories,
                 getConnectedAccessoriesWithResolver:(RCTPromiseResolveBlock)resolve
                 rejecter:(RCTPromiseRejectBlock)reject)
{
  @try {
    NSArray<EAAccessory *> *accessories = [EAAccessoryManager sharedAccessoryManager].connectedAccessories;
    NSMutableArray<NSDictionary *> *result = [NSMutableArray arrayWithCapacity:accessories.count];

    for (EAAccessory *accessory in accessories) {
      [result addObject:[self serializeAccessory:accessory]];
    }

    [self logInfo:@"scan" message:@"connected accessories fetched" extra:@{ @"count": @(result.count) }];
    resolve(result);
  } @catch (NSException *exception) {
    [self logWarn:@"scan" message:@"getConnectedAccessories exception" extra:@{ @"reason": exception.reason ?: @"unknown" }];
    reject(@"MFI_SCAN_EXCEPTION", exception.reason ?: @"MFi scan exception", nil);
  }
}

RCT_REMAP_METHOD(showBluetoothAccessoryPicker,
                 showBluetoothAccessoryPickerWithResolver:(RCTPromiseResolveBlock)resolve
                 rejecter:(RCTPromiseRejectBlock)reject)
{
  [self logInfo:@"picker" message:@"showBluetoothAccessoryPicker request" extra:nil];
  dispatch_async(dispatch_get_main_queue(), ^{
    [[EAAccessoryManager sharedAccessoryManager]
        showBluetoothAccessoryPickerWithNameFilter:nil
                                        completion:^(NSError * _Nullable error) {
      if (!error) {
        [self logInfo:@"picker" message:@"picker selected accessory" extra:nil];
        resolve(@{ @"status": @"selected" });
        return;
      }

      if (![error.domain isEqualToString:EABluetoothAccessoryPickerErrorDomain]) {
        [self logWarn:@"picker" message:@"picker failed with unexpected domain" extra:@{
          @"domain": error.domain ?: @"",
          @"code": @(error.code),
          @"message": error.localizedDescription ?: @"unknown"
        }];
        reject(@"MFI_PICKER_FAILED", error.localizedDescription ?: @"MFi picker failed", error);
        return;
      }

      EABluetoothAccessoryPickerErrorCode code = (EABluetoothAccessoryPickerErrorCode)error.code;
      switch (code) {
        case EABluetoothAccessoryPickerAlreadyConnected:
          [self logInfo:@"picker" message:@"picker already connected" extra:nil];
          resolve(@{ @"status": @"already_connected" });
          return;
        case EABluetoothAccessoryPickerResultNotFound:
          [self logWarn:@"picker" message:@"picker no accessory found" extra:nil];
          resolve(@{ @"status": @"not_found" });
          return;
        case EABluetoothAccessoryPickerResultCancelled:
          [self logInfo:@"picker" message:@"picker cancelled by user" extra:nil];
          resolve(@{ @"status": @"cancelled" });
          return;
        case EABluetoothAccessoryPickerResultFailed:
        default:
          [self logWarn:@"picker" message:@"picker failed" extra:@{
            @"code": @(error.code),
            @"message": error.localizedDescription ?: @"unknown"
          }];
          reject(@"MFI_PICKER_FAILED", error.localizedDescription ?: @"MFi picker failed", error);
          return;
      }
    }];
  });
}

RCT_REMAP_METHOD(openSession,
                 openSessionWithAccessoryId:(NSString *)accessoryId
                 protocol:(NSString *)protocol
                 resolver:(RCTPromiseResolveBlock)resolve
                 rejecter:(RCTPromiseRejectBlock)reject)
{
  [self logInfo:@"connect" message:@"openSession request" extra:@{
    @"accessoryId": accessoryId ?: @"",
    @"protocol": protocol ?: @""
  }];

  if (accessoryId.length == 0 || protocol.length == 0) {
    [self logWarn:@"connect" message:@"invalid arguments" extra:nil];
    reject(@"MFI_INVALID_ARGUMENT", @"accessoryId/protocol 不能为空", nil);
    return;
  }

  EAAccessory *accessory = [self findAccessoryByIdentifier:accessoryId];
  if (!accessory) {
    [self logWarn:@"connect" message:@"accessory not found" extra:@{ @"accessoryId": accessoryId ?: @"" }];
    reject(@"MFI_ACCESSORY_NOT_FOUND", [NSString stringWithFormat:@"未找到配件: %@", accessoryId], nil);
    return;
  }

  if (![accessory.protocolStrings containsObject:protocol]) {
    [self logWarn:@"connect" message:@"protocol not supported by accessory" extra:@{
      @"accessoryId": accessoryId ?: @"",
      @"protocol": protocol ?: @"",
      @"availableProtocols": accessory.protocolStrings ?: @[]
    }];
    reject(@"MFI_PROTOCOL_NOT_SUPPORTED", [NSString stringWithFormat:@"配件不支持协议: %@", protocol], nil);
    return;
  }

  EASession *session = [[EASession alloc] initWithAccessory:accessory forProtocol:protocol];
  if (!session || !session.inputStream || !session.outputStream) {
    [self logWarn:@"connect" message:@"EASession create failed" extra:@{
      @"accessoryId": accessoryId ?: @"",
      @"protocol": protocol ?: @""
    }];
    reject(@"MFI_OPEN_SESSION_FAILED", @"创建 EASession 失败", nil);
    return;
  }

  NSString *sessionId = [NSUUID UUID].UUIDString;
  NSInputStream *inputStream = session.inputStream;
  NSOutputStream *outputStream = session.outputStream;

  inputStream.delegate = self;
  outputStream.delegate = self;
  [inputStream scheduleInRunLoop:[NSRunLoop mainRunLoop] forMode:NSDefaultRunLoopMode];
  [inputStream scheduleInRunLoop:[NSRunLoop mainRunLoop] forMode:NSRunLoopCommonModes];
  [outputStream scheduleInRunLoop:[NSRunLoop mainRunLoop] forMode:NSDefaultRunLoopMode];
  [outputStream scheduleInRunLoop:[NSRunLoop mainRunLoop] forMode:NSRunLoopCommonModes];
  [inputStream open];
  [outputStream open];

  self.sessions[sessionId] = session;
  self.sessionAccessories[sessionId] = accessory;
  [self.inputStreamSessionIds setObject:sessionId forKey:inputStream];
  [self.outputStreamSessionIds setObject:sessionId forKey:outputStream];

  [self logInfo:@"connect" message:@"openSession success" extra:@{
    @"sessionId": sessionId ?: @"",
    @"accessoryId": accessoryId ?: @"",
    @"protocol": protocol ?: @"",
    @"accessoryName": accessory.name ?: @""
  }];
  resolve(sessionId);
}

RCT_REMAP_METHOD(send,
                 sendWithSessionId:(NSString *)sessionId
                 base64Data:(NSString *)base64Data
                 resolver:(RCTPromiseResolveBlock)resolve
                 rejecter:(RCTPromiseRejectBlock)reject)
{
  EASession *session = self.sessions[sessionId];
  if (!session || !session.outputStream) {
    [self logWarn:@"tx" message:@"session not found" extra:@{ @"sessionId": sessionId ?: @"" }];
    reject(@"MFI_SESSION_NOT_FOUND", @"会话不存在或已关闭", nil);
    return;
  }

  NSData *data = [[NSData alloc] initWithBase64EncodedString:base64Data options:0];
  if (!data || data.length == 0) {
    [self logWarn:@"tx" message:@"invalid base64 payload" extra:@{ @"sessionId": sessionId ?: @"" }];
    reject(@"MFI_INVALID_BASE64", @"发送数据不是有效 base64", nil);
    return;
  }
  NSUInteger payloadBytes = data.length;

  NSOutputStream *outputStream = session.outputStream;
  if (outputStream.streamStatus != NSStreamStatusOpen &&
      outputStream.streamStatus != NSStreamStatusWriting) {
    [self logWarn:@"tx" message:@"output stream not open" extra:@{
      @"sessionId": sessionId ?: @"",
      @"streamStatus": @(outputStream.streamStatus)
    }];
    reject(@"MFI_OUTPUT_NOT_OPEN", @"输出流未打开", outputStream.streamError);
    return;
  }

  const uint8_t *bytes = (const uint8_t *)data.bytes;
  NSUInteger offset = 0;
  NSDate *deadline = [NSDate dateWithTimeIntervalSinceNow:kOBDMFiWriteTimeoutSeconds];

  while (offset < data.length) {
    if (!outputStream.hasSpaceAvailable) {
      if ([deadline timeIntervalSinceNow] <= 0) {
        [self logWarn:@"tx" message:@"wait writable timeout" extra:@{
          @"sessionId": sessionId ?: @"",
          @"bytes": @(payloadBytes)
        }];
        reject(@"MFI_WRITE_TIMEOUT", @"输出流等待可写超时", nil);
        return;
      }
      [[NSRunLoop currentRunLoop] runMode:NSDefaultRunLoopMode beforeDate:[NSDate dateWithTimeIntervalSinceNow:0.01]];
      continue;
    }

    NSInteger written = [outputStream write:bytes + offset maxLength:data.length - offset];
    if (written < 0) {
      [self logWarn:@"tx" message:@"write failed" extra:@{
        @"sessionId": sessionId ?: @"",
        @"bytes": @(payloadBytes),
        @"error": outputStream.streamError.localizedDescription ?: @"unknown"
      }];
      reject(@"MFI_WRITE_FAILED", @"写入输出流失败", outputStream.streamError);
      return;
    }
    if (written == 0) {
      if ([deadline timeIntervalSinceNow] <= 0) {
        [self logWarn:@"tx" message:@"write timeout" extra:@{
          @"sessionId": sessionId ?: @"",
          @"bytes": @(payloadBytes)
        }];
        reject(@"MFI_WRITE_TIMEOUT", @"输出流写入超时", nil);
        return;
      }
      [[NSRunLoop currentRunLoop] runMode:NSDefaultRunLoopMode beforeDate:[NSDate dateWithTimeIntervalSinceNow:0.01]];
      continue;
    }
    offset += (NSUInteger)written;
  }

  if (offset != data.length) {
    [self logWarn:@"tx" message:@"write partial" extra:@{
      @"sessionId": sessionId ?: @"",
      @"bytes": @(payloadBytes),
      @"written": @(offset)
    }];
    reject(@"MFI_WRITE_PARTIAL", @"输出流写入不完整", nil);
    return;
  }

  NSUInteger txSequence = [self nextSequenceForSessionId:sessionId store:self.sessionTxSequence];
  [self logInfo:@"tx" message:@"send success" extra:[self debugInfoForData:data sessionId:sessionId sequence:txSequence]];
  resolve(@YES);
}

RCT_REMAP_METHOD(closeSession,
                 closeSessionWithSessionId:(NSString *)sessionId
                 resolver:(RCTPromiseResolveBlock)resolve
                 rejecter:(RCTPromiseRejectBlock)reject)
{
  (void)reject;
  [self logInfo:@"disconnect" message:@"closeSession request" extra:@{ @"sessionId": sessionId ?: @"" }];
  BOOL closed = [self closeSessionWithId:sessionId reason:@"closed_by_js"];
  resolve(@(closed));
}

- (void)stream:(NSStream *)aStream handleEvent:(NSStreamEvent)eventCode
{
  NSString *sessionId = nil;
  if ([aStream isKindOfClass:[NSInputStream class]]) {
    sessionId = [self.inputStreamSessionIds objectForKey:(NSInputStream *)aStream];
  } else if ([aStream isKindOfClass:[NSOutputStream class]]) {
    sessionId = [self.outputStreamSessionIds objectForKey:(NSOutputStream *)aStream];
  }

  if (sessionId.length == 0) {
    return;
  }

  switch (eventCode) {
    case NSStreamEventHasBytesAvailable:
      if ([aStream isKindOfClass:[NSInputStream class]]) {
        [self readInputStream:(NSInputStream *)aStream sessionId:sessionId];
      }
      break;

    case NSStreamEventErrorOccurred:
      [self logWarn:@"stream" message:@"stream error event" extra:@{
        @"sessionId": sessionId ?: @"",
        @"error": aStream.streamError.localizedDescription ?: @"stream_error"
      }];
      [self closeSessionWithId:sessionId
                        reason:(aStream.streamError.localizedDescription ?: @"stream_error")];
      break;

    case NSStreamEventEndEncountered:
      [self logWarn:@"stream" message:@"stream end encountered" extra:@{ @"sessionId": sessionId ?: @"" }];
      [self closeSessionWithId:sessionId reason:@"stream_end"];
      break;

    default:
      break;
  }
}

- (void)readInputStream:(NSInputStream *)stream sessionId:(NSString *)sessionId
{
  uint8_t buffer[4096];

  while (stream.hasBytesAvailable) {
    NSInteger count = [stream read:buffer maxLength:sizeof(buffer)];
    if (count < 0) {
      [self logWarn:@"rx" message:@"read failed" extra:@{
        @"sessionId": sessionId ?: @"",
        @"error": stream.streamError.localizedDescription ?: @"read_error"
      }];
      [self closeSessionWithId:sessionId reason:(stream.streamError.localizedDescription ?: @"read_error")];
      return;
    }

    if (count == 0) {
      return;
    }

    NSData *data = [NSData dataWithBytes:buffer length:(NSUInteger)count];
    NSString *base64 = [data base64EncodedStringWithOptions:0] ?: @"";
    NSUInteger rxSequence = [self nextSequenceForSessionId:sessionId store:self.sessionRxSequence];
    [self logInfo:@"rx" message:@"chunk received" extra:[self debugInfoForData:data sessionId:sessionId sequence:rxSequence]];

    if (self.hasListeners) {
      [self sendEventWithName:kOBDMFiDataEvent body:@{
        @"sessionId" : sessionId,
        @"base64Data" : base64
      }];
    }
  }
}

- (EAAccessory *)findAccessoryByIdentifier:(NSString *)identifier
{
  NSString *target = [identifier stringByTrimmingCharactersInSet:NSCharacterSet.whitespaceAndNewlineCharacterSet];
  NSArray<EAAccessory *> *accessories = [EAAccessoryManager sharedAccessoryManager].connectedAccessories;

  for (EAAccessory *accessory in accessories) {
    NSString *serial = accessory.serialNumber ?: @"";
    NSString *connectionId = @(accessory.connectionID).stringValue;

    if ([serial isEqualToString:target] || [connectionId isEqualToString:target]) {
      return accessory;
    }
  }

  return nil;
}

- (NSDictionary *)serializeAccessory:(EAAccessory *)accessory
{
  if (!accessory) {
    return @{
      @"name" : @"",
      @"manufacturer" : @"",
      @"modelNumber" : @"",
      @"serialNumber" : @"",
      @"connectionID" : @"",
      @"protocolStrings" : @[]
    };
  }
  return @{
    @"name" : accessory.name ?: @"",
    @"manufacturer" : accessory.manufacturer ?: @"",
    @"modelNumber" : accessory.modelNumber ?: @"",
    @"serialNumber" : accessory.serialNumber ?: @"",
    @"connectionID" : @(accessory.connectionID).stringValue,
    @"protocolStrings" : accessory.protocolStrings ?: @[]
  };
}

- (void)onAccessoryDidConnect:(NSNotification *)notification
{
  EAAccessory *connected = notification.userInfo[EAAccessoryKey];
  if (!connected) {
    connected = notification.userInfo[EAAccessorySelectedKey];
  }
  if (!connected) return;

  [self logInfo:@"connect" message:@"accessory did connect" extra:@{
    @"connectionID": @(connected.connectionID).stringValue ?: @"",
    @"name": connected.name ?: @"",
    @"protocols": connected.protocolStrings ?: @[]
  }];

  if (self.hasListeners) {
    [self sendEventWithName:kOBDMFiAccessoryConnectedEvent body:[self serializeAccessory:connected]];
  }
}

- (void)onAccessoryDidDisconnect:(NSNotification *)notification
{
  EAAccessory *disconnected = notification.userInfo[EAAccessoryKey];
  if (!disconnected) return;
  [self logWarn:@"disconnect" message:@"accessory did disconnect" extra:@{
    @"connectionID": @(disconnected.connectionID).stringValue ?: @"",
    @"name": disconnected.name ?: @""
  }];

  if (self.hasListeners) {
    [self sendEventWithName:kOBDMFiAccessoryDisconnectedEvent body:[self serializeAccessory:disconnected]];
  }

  NSArray<NSString *> *sessionIds = self.sessionAccessories.allKeys;
  for (NSString *sessionId in sessionIds) {
    EAAccessory *sessionAccessory = self.sessionAccessories[sessionId];
    if (sessionAccessory.connectionID == disconnected.connectionID) {
      [self closeSessionWithId:sessionId reason:@"accessory_disconnected"];
    }
  }
}

- (BOOL)closeSessionWithId:(NSString *)sessionId reason:(NSString *)reason
{
  EASession *session = self.sessions[sessionId];
  if (!session) {
    [self logWarn:@"disconnect" message:@"closeSession ignored: session missing" extra:@{
      @"sessionId": sessionId ?: @"",
      @"reason": reason ?: @""
    }];
    return NO;
  }

  if (session.inputStream) {
    [self.inputStreamSessionIds removeObjectForKey:session.inputStream];
    [session.inputStream close];
    [session.inputStream removeFromRunLoop:[NSRunLoop mainRunLoop] forMode:NSDefaultRunLoopMode];
    [session.inputStream removeFromRunLoop:[NSRunLoop mainRunLoop] forMode:NSRunLoopCommonModes];
    session.inputStream.delegate = nil;
  }

  if (session.outputStream) {
    [self.outputStreamSessionIds removeObjectForKey:session.outputStream];
    [session.outputStream close];
    [session.outputStream removeFromRunLoop:[NSRunLoop mainRunLoop] forMode:NSDefaultRunLoopMode];
    [session.outputStream removeFromRunLoop:[NSRunLoop mainRunLoop] forMode:NSRunLoopCommonModes];
    session.outputStream.delegate = nil;
  }

  [self.sessions removeObjectForKey:sessionId];
  [self.sessionAccessories removeObjectForKey:sessionId];
  [self.sessionTxSequence removeObjectForKey:sessionId];
  [self.sessionRxSequence removeObjectForKey:sessionId];

  [self logInfo:@"disconnect" message:@"session closed" extra:@{
    @"sessionId": sessionId ?: @"",
    @"reason": reason ?: @"session_closed"
  }];

  if (self.hasListeners) {
    [self sendEventWithName:kOBDMFiClosedEvent body:@{
      @"sessionId" : sessionId,
      @"reason" : reason ?: @"session_closed"
    }];
  }

  return YES;
}

- (void)closeAllSessionsWithReason:(NSString *)reason
{
  NSArray<NSString *> *sessionIds = self.sessions.allKeys;
  [self logInfo:@"disconnect" message:@"closeAllSessions" extra:@{
    @"count": @(sessionIds.count),
    @"reason": reason ?: @""
  }];
  for (NSString *sessionId in sessionIds) {
    [self closeSessionWithId:sessionId reason:reason ?: @"module_cleanup"];
  }
}

@end
