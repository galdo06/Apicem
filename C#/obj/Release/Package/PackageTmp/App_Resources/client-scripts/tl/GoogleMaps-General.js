var project;
var drawingManager;

var infoBubble = new InfoBubble({
});

var maximumZIndex = 100000;

function log(message) {
}

function trim11(str) {
    str = str.replace(/^\s+/, '');
    for (var i = str.length - 1; i >= 0; i--) {
        if (/\S/.test(str.charAt(i))) {
            str = str.substring(0, i + 1);
            break;
        }
    }
    return str;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function Initialize() {
    var x;
    var y;

    if (lat != null) {
        x = lon;
        y = lat;
    }
    else if (project.X == "0") {
        x = project.CityLon;
        y = project.CityLat;
    }
    else {
        var anewpoint = ConvertToLatLng(project.X, project.Y);
        x = anewpoint.x;
        y = anewpoint.y;
    }
    var zoom = 13;

    if ((project.X == "0") || (project.Y == "0")) {
        $('#btnSetProjectCenter').css('color', 'red');
        $('#btnSetPerimeterFromSIP').css('color', 'red');
        $('#btnSetPerimeterFromSIP').attr('disabled', 'disabled');

        var poid = getParameterByName("poid");
        var zoom = poid == "0" || poid == null || poid == "" ? 13 : 19;
    }
    else {
        $('#btnSetProjectCenter').css('color', 'green');
        $('#btnSetPerimeterFromSIP').css('color', 'green');
        $('#btnSetPerimeterFromSIP').removeAttr('disabled');
        zoom = 19;
    }

    var latlng = new google.maps.LatLng(y, x);
    var myOptions = {
        zoom: zoom,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.HYBRID,
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
            mapTypeIds: [
                                    google.maps.MapTypeId.ROADMAP,
                                    google.maps.MapTypeId.SATELLITE,
                                    google.maps.MapTypeId.TERRAIN,
                                    google.maps.MapTypeId.HYBRID
            ]
        },
        zoomControl: true,
        zoomControlOptions: { style: google.maps.ZoomControlStyle.LARGE },

        panControl: false,
        scaleControl: true,
        streetViewControl: false,
        overviewMapControl: false,
        rotateControl: false
    };
    mapMarker = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    mapMarker.setTilt(0);

    google.maps.event.addListener(mapMarker, 'click', function (event) {
        infoBubble.close(mapMarker);
    });


    InitializeMarkers(Trees);
    InitializePerimeters(Perimeters);

    AddDrawingManager();
    //addTreeNamesToList();
    return true;
}

function setProjectCenter() {
    if (ShowLoading)
        ShowLoading();
    var anewpoint = ConvertToStatePlane(mapMarker.center.lng(), mapMarker.center.lat());

    var myJSONText = JSON.stringify({
        X: anewpoint.x,
        Y: anewpoint.y,
        Lat: mapMarker.center.lat(),
        Lon: mapMarker.center.lng()
    });
    var apiLocation = document.domain == 'localhost' ? "" : "/web";
    $.ajax({
        type: "POST",
        url: apiLocation + "/secured/member/tl/treelocation.aspx/EditProjectCenter",
        data: "{projectID:'" + project.ProjectID + "',JStr:'" + myJSONText + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successSetProjectCenter,
        error: errorFn
    });

    project.X = anewpoint.x,
    project.Y = anewpoint.y;
    project.Lat = mapMarker.center.lat();
    project.Lon = mapMarker.center.lng();

    mapMarker.setZoom(19);

    $('#btnSetProjectCenter').css('color', 'green');
    $('#btnSetPerimeterFromSIP').css('color', 'green');
    $('#btnSetPerimeterFromSIP').removeAttr('disabled');
}

function successSetProjectCenter(result) {
    if (CloseLoading)
        CloseLoading();
    alert("El lugar del proyecto ha sido especificado satisfactoriamente.");
}

function setPerimeterFromSIP() {
    if (ShowLoading)
        ShowLoading();
    var anewpoint = ConvertToStatePlane(project.X, project.Y);

    var myJSONText = JSON.stringify({
        X: project.X,
        Y: project.Y,
        Lat: project.Lat,
        Lon: project.Lon
    });

    var apiLocation = document.domain == 'localhost' ? "" : "/web";
    $.ajax({
        type: "POST",
        url: apiLocation + "/secured/member/tl/treelocation.aspx/SetPerimeterFromSIP",
        data: "{projectID:'" + project.ProjectID + "',JStr:'" + myJSONText + "',userID:'" + editorID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successSetPerimeterFromSIP,
        error: errorFn
    });

    mapMarker.setZoom(19);
}

function successSetPerimeterFromSIP(result) {
    if (CloseLoading)
        CloseLoading();
    for (var i = (polylines.length - 1) ; i >= 0; i--) {
        if (true == polylines[i].Perimeter.IsMainPerimeter) {
            anyMainPerimeter = false;

            color = GetColorByID(perimeterDefaultColorCode);


            polylines[i].strokeColor = '#' + color.Code;
            polylines[i].fillColor = '#' + color.Code,

            polylines[i].Perimeter.PerimeterName = "Perímetro";
            polylines[i].Perimeter.Code = color.Code;
            polylines[i].Perimeter.ColorID = color.ColorID;
            polylines[i].Perimeter.ColorDesc = color.ColorDesc,
            polylines[i].Perimeter.IsMainPerimeter = false;

            setPolylineZIndex(polylines[i]);
            break;
        }
    }
    CreatePolygon(result.d);

    alert("Perímetro creado satisfactoriamente.");
}

function AddDrawingManager() {
    drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.MARKER,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [google.maps.drawing.OverlayType.MARKER
                , google.maps.drawing.OverlayType.POLYLINE
            ]
        },
        markerOptions: {
            draggable: true
        },
        polylineOptions: {
            strokeColor: '#E9425C',
            zIndex: maximumZIndex + 10000,
            strokeOpacity: 1,
            strokeWeight: 3,
            editable: true
        }
    });

    drawingManager.markerOptions.map = mapMarker;
    drawingManager.setMap(mapMarker);
    drawingManager.setDrawingMode(null);

    google.maps.event.addListener(drawingManager, 'markercomplete', function (markerTemp) {
        Redirect(markerTemp, 'new');
    });

    google.maps.event.addListener(drawingManager, 'polylinecomplete', function (perimeterTemp) {
        AddPerimeter(perimeterTemp);
    });
}

function successAddDrawingManager(result) {
    //for (var i = (polylines.length - 1) ; i >= 0; i--) {
    //    if ("0" == polylines[i].Perimeter.PerimeterID) {
    //        polylines[i].Perimeter.PerimeterID = result.d;
    //        break;
    //    }
    //}
    for (var i = (polylines.length - 1) ; i >= 0; i--) {
        if ("0" == polylines[i].Perimeter.PerimeterID) {
            polylines[i].Perimeter.PerimeterID = result.d;
            break;
        }
    }
}

function Redirect(marker, editMode) {
    var anewpoint = ConvertToStatePlane(marker.position.lng(), marker.position.lat());
    newTreeUrl = newTreeUrl
        .replace("%7Bedit_mode%7D", editMode)
        .replace("%7Bproject_organism_id%7D", (marker.tree) ? marker.tree.ProjectOrganismID : 0)
        .replace("%7Bnumber%7D", nextMarkerNumber)
        .replace("%7Bx%7D", anewpoint.x)
        .replace("%7By%7D", anewpoint.y)
        .replace("%7Blat%7D", marker.position.lat())
        .replace("%7Blon%7D", marker.position.lng());

    window.location.replace(newTreeUrl);
}

function errorFn(result) {
    if (CloseLoading)
        CloseLoading();
    alert(result);
}

function createIcon(color, label) {
    var opts = {};
    opts.primaryColor = color;
    opts.label = label;
    var image = createLabeledMarkerIcon(opts);
    return image;
}

function createLabeledMarkerIcon(opts) {
    var primaryColor = opts.primaryColor || "#DA7187";
    var strokeColor = opts.strokeColor || "#000000";
    var starPrimaryColor = opts.starPrimaryColor || "#FFFF00";
    var starStrokeColor = opts.starStrokeColor || "#0598F0";
    var label = "100";// MapIconMaker.escapeUserText_(opts.label) || "100";
    var labelColor = opts.labelColor || "#000000";
    var addStar = opts.addStar || false;

    var pinProgram = (addStar) ? "pin_star" : "pin";
    var baseUrl = "http://chart.apis.google.com/chart?cht=d&chdp=mapsapi&chl=";
    var iconUrl = baseUrl + pinProgram + "'i\\" + "'[" + label +
                              "'-2'f\\" + "hv'a\\]" + "h\\]o\\" +
                                    primaryColor.replace("#", "") + "'fC\\" +
                                         labelColor.replace("#", "") + "'tC\\" +
                                               strokeColor.replace("#", "") + "'eC\\";
    if (addStar) {
        iconUrl += starPrimaryColor.replace("#", "") + "'1C\\" +
                                                             starStrokeColor.replace("#", "") + "'0C\\";
    }
    iconUrl += "Lauto'f\\";

    var icon = {}
    icon.image = iconUrl + "&ext=.png";
    return icon.image;
};

function exit() {
    window.location = "./Main.aspx";
}

function ConvertToStatePlane(x, y) {
    Proj4js.defs["EPSG:32161"] = "+proj=lcc +lat_1=18.43333333333333 +lat_2=18.03333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=200000 +y_0=200000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs";
    Proj4js.defs["EPSG:4326"] = "+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs";
    var dest = new Proj4js.Proj('EPSG:32161');
    var source = new Proj4js.Proj('EPSG:4326');
    var anewpoint = new Proj4js.Point(x, y);
    Proj4js.transform(source, dest, anewpoint);
    return anewpoint;
}

//collects the X and the Y in state plane, and returns a point with the lat/lng in it...
function ConvertToLatLng(x, y) {
    Proj4js.defs["EPSG:32161"] = "+proj=lcc +lat_1=18.43333333333333 +lat_2=18.03333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=200000 +y_0=200000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs";
    Proj4js.defs["EPSG:4326"] = "+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs";
    var source = new Proj4js.Proj('EPSG:32161');
    var dest = new Proj4js.Proj('EPSG:4326');
    var anewpoint = new Proj4js.Point(x, y);
    Proj4js.transform(source, dest, anewpoint);
    return anewpoint;
}

function clone(obj) {
    if (null == obj || "object" != typeof obj) return obj;
    var copy = obj.constructor();
    for (var attr in obj) {
        if (obj.hasOwnProperty(attr)) copy[attr] = obj[attr];
    }
    return copy;
}
