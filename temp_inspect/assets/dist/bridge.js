function invokeCSharp() {
    if (typeof JSBridge !== 'undefined' && JSBridge !== null) {
        JSBridge.callCSharpMethod('Hello from JavaScript!');
    } else {
        console.log('JSBridge not found');
    }
}

let call_id = 0;

function checkPromise(value) {
    if (value instanceof Promise) {
        const id = call_id++;

        value
            .then(result => JSBridge.setPromiseResult(id, true, JSON.stringify(result)))
            .catch(error => JSBridge.setPromiseResult(id, false, JSON.stringify(error)));

        return { promise: id };
    }

    return { value };
}

function onOBDStatusChanged(status) {
    console.log("onOBDStatusChanged: ", status);
    const elmStatus = document.getElementById("elm-status");
    const ecuStatus = document.getElementById("ecu-status");
    const btnConnect = document.getElementById("btn-connect");
    switch (status) {
        case "ConnectingToELM":
            elmStatus.innerText = "Connecting";
            break;
        case "ConnectedToELM":
            elmStatus.innerText = "Connected";
            btnConnect.innerText = "DISCONNECT";
            btnConnect.onclick = () => JSBridge.disconnectAsync();
            break;
        case "ConnectingToECU":
            ecuStatus.innerText = "Connecting";
            break;
        case "ConnectedToECU":
            ecuStatus.innerText = "Connected";
            break;
        case "Disconnecting":
            elmStatus.innerText = "Disconnecting";
            ecuStatus.innerText = "Disconnecting";
            break;
        case "Disconnected":
            elmStatus.innerText = "Disconnected";
            ecuStatus.innerText = "Disconnected";
            btnConnect.innerText = "CONNECT";
            btnConnect.onclick = () => JSBridge.connectAsync('wifi', '192.168.0.10:35000');
            break;
    }
}

function onConnectSuccess(result) {
    console.log("onConnectSuccess", result);
}
function onConnectError(error) {
    console.error("onConnectError", error);
}
function onConnectFinish() {

}

function onDisconnectSuccess(result) {
    console.log("onDisconnectSuccess", result);
}
function onDisconnectError(error) {
    console.error("onDisconnectError", error);
}
function onDisconnectFinish() {

}

function onBTScanEvent(evt) {
    console.log("onBTScanEvent", evt);
}

function onBLEScanEvent(evt) {
    console.log("onBLEScanEvent", evt);
}

function onReadECUInfoSuccess(result) {
    console.log("onReadECUInfoSucces ", result);
}

function onReadECUInfoError(error) {
    console.log("onReadECUInfoError", error);
}
function onReadECUInfoFinish() {

}

function readECUInfo(indexList) {
    JSBridge.readECUInfoAsync(JSON.stringify(indexList));
}

function onClearDTCSuccess(result) {
    console.log("onClearDTCSuccess", result);
}

function onClearDTCError(error) {
    console.log("onClearDTCError", error);
}

function onClearDTCFinish() {
}

function clearDTC(indexList) {
    JSBridge.clearDTCAsync(JSON.stringify(indexList));
}


function onReadDTCSuccess(result) {
    console.log("onReadDTCSuccess", JSON.parse(result));
}

function onReadDTCError(error) {
    console.log("onReadDTCError", error);
}

function onReadDTCFinish() {
}

function readDTCAsync(indexList) {
    JSBridge.readDTCAsync(JSON.stringify(indexList));
}

function onReadFreezeFrameSuccess(result) {
    console.log("onReadFreezeFrameSuccess", JSON.parse(result));
}

function onReadFreezeFrameError(error) {
    console.log("onReadFreezeFrameError", error);
}

function onReadFreezeFrameFinish() {
}

function readDTCAsync(indexList) {
    JSBridge.readDTCAsync(JSON.stringify(indexList));
}


function getBrands() {
    return JSON.parse(JSBridge.getBrands());
}

function getProfiles(brand) {
    return JSON.parse(JSBridge.getProfiles(brand));
}

function applyProfile(brand, index) {
    JSBridge.applyProfile(brand, index);
}

function getECUList() {
    return JSON.parse(JSBridge.getECUList());
}

function getPIDList() {
    return JSON.parse(JSBridge.getPIDList());
}

function startReadPIDs(indexList) {
    return JSBridge.startReadPIDs(JSON.stringify(indexList));
}

function stopReadPIDs() {
    JSBridge.stopReadPIDs();
}

function onPIDValueChanged(e) {
    console.log("onPIDValueChanged", e);
}