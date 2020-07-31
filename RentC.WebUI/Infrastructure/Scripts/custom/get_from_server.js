function SaveJSON_ToStorage(key, JSON_text) {
        console.log("It works!!!");
        localStorage.setItem(key, JSON_text);
}


function RemoveFromStorage(key) {
    console.log("RFS invoked");
    localStorage.removeItem(key);
}
