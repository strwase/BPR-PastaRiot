function callFunctionApp(
    method: string,
    url: string,
    successCallback: Function,
    errorCallback: Function,
    body: string
): any {
    var request = new XMLHttpRequest();
    request.open(method, url, true);
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.onreadystatechange = function () {
        if (this.readyState === 4) {
            request.onreadystatechange = null;
            if (this.status === 200) {
                if (successCallback != undefined) {
                    successCallback(this.response);
                } else {
                    console.log("call succesfull..");
                }
            } else {
                if (errorCallback != undefined) {
                    errorCallback(this.statusText);
                } else {
                    console.log("call failed: " + this.statusText);
                }
            }
        }
    };
    request.send(body);
}