#!/bin/sh
pushd "$PODS_ROOT/../" > /dev/null
RCT_SCRIPT_POD_INSTALLATION_ROOT=$(pwd)
popd >/dev/null

export RCT_SCRIPT_RN_DIR=${PODS_TARGET_SRCROOT}/..
export RCT_SCRIPT_LIBRARY_NAME=rncore
export RCT_SCRIPT_OUTPUT_DIR=$RCT_SCRIPT_POD_INSTALLATION_ROOT/../node_modules/react-native/ReactCommon
export RCT_SCRIPT_LIBRARY_TYPE=components
export RCT_SCRIPT_JS_SRCS_PATTERN=*NativeComponent.js
export RCT_SCRIPT_JS_SRCS_DIR=../Libraries
export RCT_SCRIPT_CODEGEN_MODULE_DIR=../node_modules/react-native/ReactCommon/.
export RCT_SCRIPT_CODEGEN_COMPONENT_DIR=../node_modules/react-native/ReactCommon/react/renderer/components
export RCT_SCRIPT_FILE_LIST="[\"../Libraries/Components/ActivityIndicator/ActivityIndicatorViewNativeComponent.js\", \"../Libraries/Components/DrawerAndroid/AndroidDrawerLayoutNativeComponent.js\", \"../Libraries/Components/ProgressBarAndroid/ProgressBarAndroidNativeComponent.js\", \"../Libraries/Components/RefreshControl/AndroidSwipeRefreshLayoutNativeComponent.js\", \"../Libraries/Components/RefreshControl/PullToRefreshViewNativeComponent.js\", \"../Libraries/Components/SafeAreaView/RCTSafeAreaViewNativeComponent.js\", \"../Libraries/Components/ScrollView/AndroidHorizontalScrollContentViewNativeComponent.js\", \"../Libraries/Components/ScrollView/AndroidHorizontalScrollViewNativeComponent.js\", \"../Libraries/Components/ScrollView/ScrollContentViewNativeComponent.js\", \"../Libraries/Components/ScrollView/ScrollViewNativeComponent.js\", \"../Libraries/Components/Switch/AndroidSwitchNativeComponent.js\", \"../Libraries/Components/Switch/SwitchNativeComponent.js\", \"../Libraries/Components/TextInput/AndroidTextInputNativeComponent.js\", \"../Libraries/Components/TextInput/RCTInputAccessoryViewNativeComponent.js\", \"../Libraries/Components/TextInput/RCTMultilineTextInputNativeComponent.js\", \"../Libraries/Components/TextInput/RCTSingelineTextInputNativeComponent.js\", \"../Libraries/Components/TraceUpdateOverlay/TraceUpdateOverlayNativeComponent.js\", \"../Libraries/Components/UnimplementedViews/UnimplementedNativeViewNativeComponent.js\", \"../Libraries/Components/View/ViewNativeComponent.js\", \"../Libraries/Image/ImageViewNativeComponent.js\", \"../Libraries/Image/TextInlineImageNativeComponent.js\", \"../Libraries/Modal/RCTModalHostViewNativeComponent.js\", \"../Libraries/ReactNative/requireNativeComponent.js\", \"../Libraries/Text/TextNativeComponent.js\", \"../Libraries/Utilities/codegenNativeComponent.js\"]"

SCRIPT_PHASES_SCRIPT="$RCT_SCRIPT_RN_DIR/scripts/react_native_pods_utils/script_phases.sh"
WITH_ENVIRONMENT="$RCT_SCRIPT_RN_DIR/scripts/xcode/with-environment.sh"
/bin/sh -c "$WITH_ENVIRONMENT $SCRIPT_PHASES_SCRIPT"

