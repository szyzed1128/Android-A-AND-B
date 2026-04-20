#import <Foundation/Foundation.h>
#import <React/RCTBridgeModule.h>

@interface LogBridge : NSObject <RCTBridgeModule>
@end

@implementation LogBridge

RCT_EXPORT_MODULE();

+ (BOOL)requiresMainQueueSetup
{
  return NO;
}

RCT_EXPORT_METHOD(log:(NSString *)level message:(NSString *)message)
{
  NSString *safeLevel = level ?: @"info";
  NSString *safeMessage = message ?: @"";
  NSLog(@"[RNLogBridge][%@] %@", safeLevel, safeMessage);
}

@end
