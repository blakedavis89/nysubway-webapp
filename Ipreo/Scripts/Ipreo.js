function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {
    var R = 6371; // Radius of the earth in km
    var dLat = deg2rad(lat2 - lat1);  // deg2rad below
    var dLon = deg2rad(lon2 - lon1);
    var a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
      Math.sin(dLon / 2) * Math.sin(dLon / 2)
    ;
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c; // Distance in km

    displayResults(d);
}

function deg2rad(deg) {
    return deg * (Math.PI / 180)
}

function displayResults(distance)
{
    var divResult = document.getElementById("divResult");
    divResult.innerHTML = "";

    var stationA = document.getElementById("MainContent_ddlStationA");
    var stationB = document.getElementById("MainContent_ddlStationB");

    var results = "<span class=\"label label-success\">Result</span>";
    results += "<h4>The distance between " + stationA.value + " and " + stationB.value + " is ";
    results += distance.toFixed(2);
    results += " kilometers!";

    divResult.innerHTML = results;
}

function displayError(errorCode)
{
    var divResult = document.getElementById("divResult");
    divResult.innerHTML = "";

    var results = "<span class=\"label label-danger\">Error</span>";

    if (errorCode == "1")
        results += "<h4>A selection must be made for both subway stations in order to calculate their distance apart.</h4>";
    else if (errorCode == "0")
        results += "<h4>Caching error during application start.  Please refer to the ErrorLog.txt in the Logs directory for more information.</h4>";
    else
        results += "<h4>An error has occured.</h4>";

    divResult.innerHTML = results;
}