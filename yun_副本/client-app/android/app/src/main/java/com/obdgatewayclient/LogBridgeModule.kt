package com.obdgatewayclient

import android.util.Log
import com.facebook.react.bridge.Promise
import com.facebook.react.bridge.ReactApplicationContext
import com.facebook.react.bridge.ReactContextBaseJavaModule
import com.facebook.react.bridge.ReactMethod

class LogBridgeModule(reactContext: ReactApplicationContext) : ReactContextBaseJavaModule(reactContext) {
    override fun getName(): String = "LogBridge"

    @ReactMethod
    fun log(level: String, message: String) {
        when (level.lowercase()) {
            "warn" -> Log.w(TAG, message)
            "error" -> Log.e(TAG, message)
            else -> Log.i(TAG, message)
        }
    }

    @ReactMethod
    fun ping(promise: Promise) {
        promise.resolve("ok")
    }

    companion object {
        private const val TAG = "RNLogBridge"
    }
}
