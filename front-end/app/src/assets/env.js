(function (window) {
    window["env"] = window["env"] || {};
  
    // Environment variables
    window["env"]["api"] = "${API}" || "https://localhost:5002/api/"; 
})(this);
  