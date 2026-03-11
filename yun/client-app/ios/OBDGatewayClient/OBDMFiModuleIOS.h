#import <Foundation/Foundation.h>
#import <React/RCTBridgeModule.h>
#import <React/RCTEventEmitter.h>

@interface OBDMFiModuleIOS : RCTEventEmitter <RCTBridgeModule, NSStreamDelegate>
@end
