
var polylines = [];
var currentPolyline;
var flightPlanCoordinates = [];
var anyMainPerimeter = false;
var perimeterReserverColorCode = "2";
var perimeterDefaultColorCode = "1";

function InitializePerimeters(Perimeters) {

    for (var i = 0; i < Perimeters.length; i++) {
        CreatePolygon(Perimeters[i]);
    }
}

function CreatePolygon(polygon) {
    flightPlanCoordinates = [];
    $('#btnPolylineEdits').removeAttr('disabled');
    for (var e = 0; e < polygon.PerimeterPoints.length; e++) {
        var point = polygon.PerimeterPoints[e];
        latlng = new google.maps.LatLng(point.Y, point.X);
        flightPlanCoordinates.push(latlng);
    }

    // Create a poligon instead of a polyline
    var existingPolygon = new google.maps.Polygon({
        paths: flightPlanCoordinates,
        strokeColor: '#' + polygon.Code,
        strokeOpacity: 0.50,
        strokeWeight: 2,
        fillColor: '#' + polygon.Code,
        fillOpacity: 0.20,
        Perimeter: polygon
    });

    if (polygon.IsMainPerimeter) {
        anyMainPerimeter = true;
    }

    maximumZIndex += 1;
    existingPolygon.setMap(mapMarker);

    polylines.push(existingPolygon);
    currentPolyline = existingPolygon;

    currentPolyline.zIndex = maximumZIndex;

    addPolylineListeners(existingPolygon);
}

function addPolylineListeners(polygon) {
    // when editing started
    google.maps.event.addListener(polygon, 'edit_start', function () {
        log("[edit_start]");
    });

    // when editing in finished
    google.maps.event.addListener(polygon, 'edit_end', function (path) {
        var coords = [];
        path.forEach(function (position) {
            coords.push(position.toUrlValue(5));
        }); 
        // ERAMOS 20140421
        // var polylines = polyline.latLngs.b[0].b;
        var polylines = polyline.latLngs.getArray()[0].getArray();
        var polylinesStatePlanes = polylines;

        var i = 0;
        while (polylines[i] != undefined) {
            var anewpoint = ConvertToStatePlane(polylines[i].lng(), polylines[i].lat());
            polylinesStatePlanes[i].lng() = anewpoint.x
            polylinesStatePlanes[i].lat() = anewpoint.y;
            i++;
        }

        $.ajax({
            type: "POST",
            url: "GoogleMaps.aspx/EditPerimeterPointFromObject",
            data: "{projectID:'" + projectID + "',perimeterPoints:'" + polylinesStatePlanes + "',perimeterGroupID:'" + polyline.perimeterGroupID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: errorFn
        });

        var i = 0;
        while (polylines[i] != undefined) {
            var anewpoint = ConvertToLatLng(polylines[i].lng(), polylines[i].lat());
            polylinesStatePlanes[i].lng() = anewpoint.x
            polylinesStatePlanes[i].lat() = anewpoint.y;
            i++;
        }

        log("[edit_end]   path: " + coords.join(" | "));
    });

    // when a single point has been moved
    google.maps.event.addListener(polygon, 'update_at', function (index, position) {
        log("[update_at]  index: " + index + " position: " + position);
    });

    // when a new point has been added
    google.maps.event.addListener(polygon, 'insert_at', function (index, position) {
        setPolylineZIndex(polygon);
        log("[insert_at]  index: " + index + " position: " + position);
    });

    // when a point was deleted
    google.maps.event.addListener(polygon, 'remove_at', function (index, position) {
        log("[remove_at]  index: " + index + " position: " + position);
    });

    google.maps.event.addListener(polygon, 'dblclick', function (event) {
        currentPolyline = polygon;
        openInfoWindowPolygon(polygon, event)

    });

    google.maps.event.addListener(polygon, 'click', function (event) {
        currentPolyline = polygon;
    });

    google.maps.event.addListener(polygon, 'mouseover', function () {
    });
}

function openInfoWindowPolygon(polygon, event) {
    infoBubble.close(mapMarker, currentMarker);
    infoBubble.unbindAll();
    infoBubble.setMaxWidth(440);
    infoBubble.setMinWidth(440);
    infoBubble.setMinHeight(200);
    infoBubble.setMaxHeight(200);
    infoBubble.setPosition(event.latLng);


    var select = '<select id="selColor" style="width: 160px;" ' + ((polygon.Perimeter.IsMainPerimeter) ? 'disabled="disabled" ' : '') + ' >';
    for (var i = (Colors.length - 1) ; i >= 0; i--) {
        select += ' <option style="font-weight:bolder; background-color: #' + Colors[i].Code + ';" label="' + Colors[i].ColorDesc +
                         '" value="' + Colors[i].ColorID +
                         '" ' + ((polygon.Perimeter.ColorID == Colors[i].ColorID) ? 'selected="selected" ' : '') +
                         ((Colors[i].ColorID == perimeterReserverColorCode) ? 'disabled="disabled" ' : '') +
                         ' ></option> ';
    }
    select += '</select> ';

    infoBubble.setContent(
        '<div id="content" style="display:block; overflow:none; height:150px; " >' +
        '   <h1 class="title-regular clearfix" style="margin-bottom:00px;">' +
        '           <span class="grid_15 alpha">' + (((polygon.Perimeter.PerimeterName) && (polygon.Perimeter.PerimeterName != '')) ? polygon.Perimeter.PerimeterName : 'New Perimeter') + ' </span>' + //' + polygon.Perimeter.PerimeterID + '
        '           <span class="grid_3 omega align-right" />' +
        '   </h1>' +
        '       <div class="grid_10 inline alpha" style="width: 18em; float: left;">' +
        '           <div style="margin-top: 10px; margin-bottom: 10px;">' +
        '               <b>Nombre:</b> ' + "<input type='text' id='inPerimeterName' " + ((polygon.Perimeter.IsMainPerimeter) ? 'disabled="disabled" ' : '') + " maxlength='100' style='width: 160px;' class='text' value='" + ((polygon.Perimeter.PerimeterName) ? polygon.Perimeter.PerimeterName : '') + "' />" +//listnersMarker.tree.CommonNameDesc + '' +
        '               <br /><b>Perímetro Principal:</b> ' + "<input id='chkIsPerimeter' onClick='isPerimeterChecked(this.checked)' type='checkbox' " + ((!anyMainPerimeter || polygon.Perimeter.IsMainPerimeter) ? '' : 'disabled="disabled" ') + " " + ((polygon.Perimeter.IsMainPerimeter) ? "checked='checked'" : '') + " />" + //listnersMarker.tree.ScientificNameDesc + '' +
        '               <br /><b>Color:</b> ' + select +
        '           </div>' +
        '       </div>' +
        '       <div class="grid_9 inline omega" style="width: 14em; float: right;">' +
        '          <ul id="menu4" style="width: 14em;">' +
        '               <li> ' +
        '                   <ul  style="width: 14em;">' +
        '                       <li>' +
        '                           <a href="javascript:EditPolyline(' + polygon.Perimeter.PerimeterID + ');">' + ((currentPolyline.getEditable()) ? 'Parar de Editar y Guardar' : 'Editar') + '</a>' +
        '                       </li>' +
        '                       <li>' +
        '                           <a href="javascript:AskDeletePolyline(' + polygon.Perimeter.PerimeterID + ',' + polygon.Perimeter.ProjectID + ');">Eliminar</a>' +
        '                       </li>' +
        '                   </ul>' +
        '               </li>' +
        '               <li>' +
        '                   <a href="javascript:infoBubble.close(mapMarker);">Cerrar</a>' +
        '               </li>' +
        '               <li>' +
        '                   <a href="javascript:EditPerimeter(' + polygon.Perimeter.PerimeterID + ');">Guardar y Cerrar</a>' +
        '               </li>' +
        '           </ul>' +
        '       </div> ' +
        '   <h1 class="title-regular clearfix">' +
        '   </h1>' +
        '</div> '
    );
    if (isFunction(infoBubble.hideCloseButton))
        infoBubble.hideCloseButton();

    infoBubble.open(mapMarker);
}

function isFunction(functionToCheck) {
    var getType = {};
    return functionToCheck && getType.toString.call(functionToCheck) == '[object Function]';
}

function AddPerimeter(perimeterTemp) {
    // ERAMOS 20140421 
    // var perimeterTempPoint = perimeterTemp.latLngs.b[0].b;
    var perimeterTempPoint = perimeterTemp.latLngs.getArray()[0].getArray();

    //Delete the polyline
    perimeterTemp.setMap(null);

    var perimeterID = 0;

    var myJSONText = JSON.stringify({
        // ERAMOS 20140421 
        // perimeterPoints: perimeterTemp.latLngs.b[0].b
        perimeterPoints: perimeterTemp.latLngs.getArray()[0].getArray()
    });
    var apiLocation = document.domain == 'localhost' ? "" : "/web";
    $.ajax({
        type: "POST",
        url: apiLocation + "/secured/member/tl/treelocation.aspx/AddEditPerimeter",
        data: "{projectID:'" + project.ProjectID +
             "',perimeterID:'0" +
             "',perimeterName:'" + //perimeterID +
             "',isMainPerimeter:'false" +
             "',colorID:'1" + //Azul
             "',perimeterPoints:'" + myJSONText +
             "',userID:'" + editorID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successAddDrawingManager,
        error: errorFn
    });

    // Create a poligon instead of a polyline
    polygon = new google.maps.Polygon({
        paths: perimeterTempPoint,
        strokeColor: '#3FCDFF',
        strokeOpacity: 0.50,
        strokeWeight: 2,
        fillColor: '#3FCDFF',
        fillOpacity: 0.20,
        Perimeter: {
            PerimeterID: perimeterID,
            ProjectID: project.ProjectID,
            Code: '#3FCDFF',
            ColorID: "1",
            IsMainPerimeter: false
        }
    }); 

    polygon.setMap(mapMarker);

    polylines.push(polygon);
    currentPolyline = polygon;

    addPolylineListeners(polygon);
    drawingManager.setDrawingMode(null);
    setPolylineZIndex(polygon);
}


function GetColorByID(colorID) {
    for (var i = (Colors.length - 1) ; i >= 0; i--) {
        if (colorID == Colors[i].ColorID) {
            return Colors[i];
        }
    }
    return null;
}

function EditPerimeter(perimeterID) {
    for (var i = (polylines.length - 1) ; i >= 0; i--) {
        if (perimeterID == polylines[i].Perimeter.PerimeterID) {
            currentPolyline = polylines[i];
            break;
        }
    }
     
    if (document.getElementById("chkIsPerimeter").checked)
        anyMainPerimeter = true;
    else
        if (currentPolyline.Perimeter.IsMainPerimeter)
            anyMainPerimeter = false;

    color = GetColorByID(document.getElementById("selColor")[document.getElementById("selColor").selectedIndex].value);

    currentPolyline.strokeColor = '#' + color.Code;
    currentPolyline.fillColor = '#' + color.Code,
    currentPolyline.Perimeter = {
        PerimeterName: document.getElementById('inPerimeterName').value,
        PerimeterID: perimeterID,
        ProjectID: project.ProjectID,
        Code: color.Code,
        ColorID: color.ColorID,
        ColorDesc: color.ColorDesc,
        IsMainPerimeter: document.getElementById("chkIsPerimeter").checked
    }

    var myJSONText = JSON.stringify({
        // ERAMOS 20140421
        // perimeterPoints: currentPolyline.latLngs.b[0].b
        perimeterPoints: currentPolyline.latLngs.getArray()[0].getArray()
    });

    var apiLocation = document.domain == 'localhost' ? "" : "/web";
    $.ajax({
        type: "POST",
        url: apiLocation + "/secured/member/tl/treelocation.aspx/AddEditPerimeter",
        data: "{projectID:'" + project.ProjectID +
             "',perimeterID:'" + perimeterID +
             "',perimeterName:'" + currentPolyline.Perimeter.PerimeterName +
             "',isMainPerimeter:'" + currentPolyline.Perimeter.IsMainPerimeter +
             "',colorID:'" + color.ColorID +
             "',perimeterPoints:'" + myJSONText +
             "',userID:'" + editorID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: errorFn
    });

    infoBubble.close(mapMarker);

    var options = {
        zIndex: currentPolyline.zIndex
    }
    currentPolyline.setOptions(options);
    
   // setPolylineZIndex(currentPolyline);
}

// start edit mode with given "ghost" option
function setPolylineZIndex(polyline) {
    maximumZIndex++;
    var options = {
        zIndex: maximumZIndex
    }
    polyline.setOptions(options);
}

function isPerimeterChecked(checked) {
    if (checked) {
        $('#inPerimeterName').val('Perímetro Principal');
        $('#selColor')[0][5].selected = true;
        $('#selColor').attr('disabled', 'disabled');
        $('#inPerimeterName').attr('disabled', 'disabled');
    }
    else {
        $('#inPerimeterName').val('Perímetro');
        $('#inPerimeterName').removeAttr('disabled');
        $('#selColor').removeAttr('disabled');
        $('#selColor')[0][6].selected = true;
    }
}

function AskDeletePolyline(perimeterID) {
    if (confirm("¿Desea eliminar este perímetro?")) {
        SetDeletePolyline(perimeterID);
    }
}

function SetDeletePolyline(perimeterID) {
    for (var i = (polylines.length - 1) ; i >= 0; i--) {
        if (perimeterID == polylines[i].Perimeter.PerimeterID) {
            if (polylines[i].Perimeter.IsMainPerimeter) {
                anyMainPerimeter = false;
            }
            infoBubble.close(mapMarker);
            polylines[i].setMap(null);
            polylines.splice(i, 1);
        }
    }
     
    var apiLocation = document.domain == 'localhost' ? "" : "/web";
    $.ajax({
        type: "POST",
        url: apiLocation + "/secured/member/tl/treelocation.aspx/DeletePerimeter",
        data: "{perimeterID:'" + perimeterID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: errorFn
    });

    return false;
}

function EditPolyline(perimeterID) {
    for (var i = (polylines.length - 1) ; i >= 0; i--) {
        if (perimeterID == polylines[i].Perimeter.PerimeterID) {
            if (polylines[i].Perimeter.IsMainPerimeter) {
                currentPolyline = polylines[i];
                anyMainPerimeter = false;
            }
        }
    }

    if (currentPolyline.getEditable()) {

        var myJSONText = JSON.stringify({
            
            // ERAMOS 20140421
            //perimeterPoints: currentPolyline.latLngs.b[0].b
            perimeterPoints: currentPolyline.latLngs.getArray()[0].getArray()
        });
         
        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        $.ajax({
            type: "POST",
            url: apiLocation + "/secured/member/tl/treelocation.aspx/AddEditPerimeter",
            data: "{projectID:'" + project.ProjectID +
                 "',perimeterID:'" + perimeterID +
                 "',perimeterName:'" + currentPolyline.Perimeter.PerimeterName +
                 "',isMainPerimeter:'" + currentPolyline.Perimeter.IsMainPerimeter +
                 "',colorID:'" + currentPolyline.Perimeter.ColorID +
                 "',perimeterPoints:'" + myJSONText +
                 "',userID:'" + editorID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: errorFn
        });

        infoBubble.close(mapMarker);
        currentPolyline.setEditable(false);
    }
    else
        currentPolyline.setEditable(true);

    infoBubble.close(mapMarker);
}
