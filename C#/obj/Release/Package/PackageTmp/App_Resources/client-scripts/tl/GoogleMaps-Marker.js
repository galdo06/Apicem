var mapMarker;
var markerPoints = [];
var anchoCounter = 1;
var currentMarker;
var currentPagerMarker;
var isTreeList = false;
var currentSort;
var currentFilter;
var currentSearch;
var maxRowCountMarker = 16;
var markers;
var nextMarkerNumber = 0;
var edit = 'edit';

// Create an ElevationService
//var elevator = new google.maps.ElevationService();

function InitializeMarkers(Trees) {
    // Marker
    nextMarkerNumber = Trees.length + 1;
    for (var i = 0; i < Trees.length; i++) {
        var tree = Trees[i];

        var anewpoint = ConvertToLatLng(tree.X, tree.Y);
        latlng = new google.maps.LatLng(anewpoint.y, anewpoint.x);

        if (tree.CommonNameDesc == "Desconocido")
            tree.Code = 'DAA8DD';

        var commentary = (tree.Dap_Counter == 1)
                   ? trim11(tree.Commentary)
                   : trim11(tree.Commentary).replace("Ramificado desde base. ", "");

        var marker = new MarkerWithLabel({
            //icon: '../App_Resources/images/icons/' + color + tree.Number + '.png',
            icon: 'https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|' + tree.Code + '|11|_|',// + tree.Number,
            //gadraggable: true,
            position: latlng,
            zIndex: maximumZIndex,
            animation: '',// google.maps.Animation.DROP,
            map: mapMarker,
            title: tree.Number.toString(),
            tree: tree,
            /* MarkerWithLabel */
            labelContent: tree.Number,
            labelAnchor: new google.maps.Point(20, 48),
            labelClass: "labels11", // the CSS class for the label  ,
            star: false, // Show star
            raiseOnDrag: false
            /* MarkerWithLabel END */
        });

        if (projectorganismid && tree.ProjectOrganismID == projectorganismid) {
            marker.zIndex = 300000;
            openInfoWindowMarker(marker);
            currentMarker = marker;
        }

        maximumZIndex += 1;
        markerPoints.push(marker);
        addMarkerListeners(marker, mapMarker, true);
        currentMarker = projectorganismid ? currentMarker : marker;
    }
    //--

    var divPrev = document.getElementById('divPrev')
    var divCurr = document.getElementById('divCurr')
    var divNext = document.getElementById('divNext')

    if (currentMarker)
        SetCurrentMarker(currentMarker, true);
}


function editMarkerProperties(isNew) {
    CreateAddMarkerModalPopup(currentMarker, false);
}

function addMarkerListeners(listnersMarker) {
    google.maps.event.addListener(listnersMarker, 'click', function () {
        listnersMarker.setZIndex(maximumZIndex + 1);
        maximumZIndex += 1;
        if (currentMarker.tree.Number == listnersMarker.tree.Number) {
            infoBubble.close(mapMarker, listnersMarker);
        }
    });

    //google.maps.event.addListener(listnersMarker, 'mouseover', function () {
    //    listnersMarker.setZIndex(maximumZIndex + 1);
    //    maximumZIndex += 1;
    //});

    google.maps.event.addListener(listnersMarker, 'dblclick', function () {
        if (currentMarker) {
            if (currentMarker.tree.Number == listnersMarker.tree.Number) {
                openInfoWindowMarker(listnersMarker);
            }
        }
        SetCurrentMarker(listnersMarker, true);
    });

    google.maps.event.addListener(listnersMarker, 'dragend', function () {
        var anewpoint = ConvertToStatePlane(this.getPosition().lng(), this.getPosition().lat());
        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        $.ajax({
            type: "POST",
            url: apiLocation + "/secured/member/tl/treelocation.aspx/EditTreePosition",
            data: "{projectOrganismID:'" + listnersMarker.tree.ProjectOrganismID + "',x:'" + anewpoint.x + "',y:'" + anewpoint.y + "',lat:'" + this.getPosition().lat() + "',lon:'" + this.getPosition().lng() + "',userID:'" + editorID + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: errorFn
        });
    });
}

function mpAddMarkerCancel(title, isNew) {
    if (isNew) {
        setDeleteMarker(title, true);
        drawingManager.setDrawingMode(google.maps.drawing.OverlayType.MARKER);
    }
    ModalPopups.Cancel("mpAddMarker");
    infowindow.close(mapMarker);
    if (isTreeList) {
        openTreeList();
    }
}

function openInfoWindowMarker(listnersMarker) {
    currentMarker = listnersMarker;
    infoBubble.close(mapMarker);
    infoBubble = new InfoBubble({
        maxWidth: 420,
        minWidth: 420
    });

    infoBubble.setContent(
        '<div id="content" style="display:block; overflow:none; height:150px; " >' +
        '   <h1 class="title-regular clearfix" style="margin-bottom:00px;">' +
        '           <span class="grid_15 alpha">Árbol ' + listnersMarker.tree.Number + '</span>' +
        '           <span class="grid_3 omega align-right" />' +
        '   </h1>' +
        '       <div class="grid_10 inline alpha" style="width: 18em; float: left;">' +
        '           <div style="margin-top: 10px; margin-bottom: 10px;">' +
        '               <b>Nom Común:</b> ' + listnersMarker.tree.CommonNameDesc + '' +
        '               <br /><b>Nom Científico:</b> ' + listnersMarker.tree.ScientificNameDesc + '' +
        '               <br /><b>Alto:</b> ' + listnersMarker.tree.Height + "'" +
        (
        (parseInt(listnersMarker.tree.Varas) > 0) ?
        '               <br /><b>Varas:</b> ' + listnersMarker.tree.Varas + ''
        :
        '               <br /><b>D.A.P:</b> ' + listnersMarker.tree.Dap + '"' +
        '               <br /><b>Troncos:</b> ' + listnersMarker.tree.Dap_Counter + ''
        ) +
        '               <br /><b>Acción:</b> ' + listnersMarker.tree.ActionProposedDesc +
        '           </div>' +
        '       </div>' +
        '       <div class="grid_9 inline omega" style="width: 12em; float: right;">' +
        '          <ul id="menu4" style="width: 12em;">' +
        '               <li> ' +
        '                   <ul  style="width: 12em;">' +
        '                       <li>' +
        '                           <a href="javascript:Redirect(currentMarker, edit);">Editar Info. Del Árbol</a>' +
        '                       </li>' +
        '                       <li>' +
        '                           <a href="javascript:mapMarker.setZoom(19);">Zoom</a>' +
        '                       </li>' +
        '                       <li>' +
        '                           <a href="javascript:AskDeleteMarker(' + listnersMarker.tree.ProjectOrganismID + ',' + listnersMarker.tree.ProjectID + ');">Eliminar</a>' +
        '                       </li>' +
        '                   </ul>' +
        '               </li>' +
        '               <li>' +
        '                   <a href="javascript:infoBubble.close(mapMarker);">Cerrar</a>' +
        '               </li>' +
        '           </ul>' +
        '       </div> ' +
        '   <h1 class="title-regular clearfix">' +
        '   </h1>' +
        '</div> '
    );
    infoBubble.hideCloseButton();
    if (parseInt(listnersMarker.tree.Varas) > 0) {
        infoBubble.setMinHeight(190);
        infoBubble.setMaxHeight(190);
    }
    else {
        infoBubble.setMinHeight(168);
        infoBubble.setMaxHeight(168);
    }

    var darwin = new google.maps.LatLng(listnersMarker.getPosition().lat(), listnersMarker.getPosition().lng());
    mapMarker.setCenter(darwin);
    infoBubble.open(mapMarker, listnersMarker);
}

function AskDeleteMarker(projectOrganismID, projectID) {
    if (confirm("¿Desea eliminar este árbol?")) {
        setDeleteMarker(projectOrganismID, projectID);
    }
}

function setDeleteMarker(projectOrganismID, projectID) {
    for (var i = (markerPoints.length - 1) ; i >= 0; i--) {
        if (projectOrganismID == markerPoints[i].tree.ProjectOrganismID) {
            infoBubble.close(mapMarker);
            markerPoints[i].setMap(null);
            markerPoints.splice(i, 1);
        }
    }

    if (markerPoints[markerPoints.length - 1])
        SetCurrentMarker(markerPoints[markerPoints.length - 1]);
    else {
        currentMarker = null;
        var divPrev = document.getElementById('divPrev');
        var divCurr = document.getElementById('divCurr');
        var divNext = document.getElementById('divNext');

        divPrev.innerHTML = "";
        divCurr.innerHTML = "";
        divNext.innerHTML = "";

        var imgPrev = document.getElementById('imgPrev');
        var imgCurr = document.getElementById('imgCurr');
        var imgNext = document.getElementById('imgNext');

        imgPrev.src = "";
        imgCurr.src = "";
        imgNext.src = "";
    }

    var apiLocation = document.domain == 'localhost' ? "" : "/web";
    $.ajax({
        type: "POST",
        url: apiLocation + "/secured/member/tl/treelocation.aspx/DeleteTree",
        data: "{projectOrganismID:'" + projectOrganismID + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: errorFn
    });

    for (var i = 0; i < markerPoints.length; i++) {

        var color;

        if (markerPoints[i].tree.ActionProposedID == "1") { // Corte y Remoción
            color = "Red/red "; // Rojo
        }
        else if (markerPoints[i].tree.ActionProposedID == "2") { // Protección
            color = "Green/green "; // Verde
        }
        else if (markerPoints[i].tree.ActionProposedID == "3") { // Poda
            color = "Orange/orange "; // Anaranjado
        }
        else if (markerPoints[i].tree.ActionProposedID == "4") { // Transplantar
            color = "Yellow/yellow "; // Amarillo
        }
        else if (markerPoints[i].tree.ActionProposedID == "5") { // Determinar Luego
            color = "Blue/blue "; // Azul
        }

        if (markerPoints[i].tree.CommonNameDesc == "Desconocido") {
            color = "Violet/violet "; // Violeta
        }

        markerPoints[i].tree.Number = i + 1;
        markerPoints[i].title = i + 1;

        //markerPoints[i].setIcon('../App_Resources/images/icons/' + color + markerPoints[i].tree.Number + '.png');
        markerPoints[i].setIcon('https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|' + color + '|11|_|');// + markerPoints[i].tree.Number.toString() );
    }

    $('.infoi').toggle(markerPoints.length > 0);

    return false;
}




function SetCurrentMarker(_marker, _centerIt, _toggleSetDraggable) {
    infoBubble.close(mapMarker);
    maximumZIndex = (maximumZIndex + 1);

    if (currentMarker && currentMarker.tree.Number != _marker.tree.Number) { // Viejo
        // Quitar el texto en BOLD al viejo
        currentMarker.setIcon("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|" + currentMarker.tree.Code + '|11|_|');// + currentMarker.tree.Number.toString());
        //
        currentMarker.setDraggable(false);
        /* MarkerWithLabel END */
        currentMarker.set("labelAnchor", new google.maps.Point(20, 48));
        currentMarker.set('star', false);
        currentMarker.set('labelClass', "labels11"); // has to be at the end
        /* MarkerWithLabel END */
        setDraggableFalse();
    }

    currentMarker = _marker;
    currentMarker.setZIndex(maximumZIndex);

    if (_toggleSetDraggable)
        currentMarker.setDraggable(!currentMarker.getDraggable());

    // Poner el texto en BOLD al nuevo
    currentMarker.setIcon("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.90|0|" + currentMarker.tree.Code + '|16|b|');// + currentMarker.tree.Number.toString());
    //

    /* MarkerWithLabel END */
    currentMarker.set("labelAnchor", new google.maps.Point(20, 54));
    currentMarker.set('star', currentMarker.getDraggable());
    currentMarker.set('labelClass', "labels15"); // has to be at the end
    /* MarkerWithLabel END */

    if (_centerIt) {
        var latlng = new google.maps.LatLng(currentMarker.getPosition().lat(), currentMarker.getPosition().lng());
        mapMarker.setCenter(latlng);
    }

    var divPrev = document.getElementById('divPrev');
    var divCurr = document.getElementById('divCurr');
    var divNext = document.getElementById('divNext');

    var imgPrev = document.getElementById('imgPrev');
    var imgCurr = document.getElementById('imgCurr');
    var imgNext = document.getElementById('imgNext');

    // <img id="imgPrev" src="../../../App_Resources/images/Left.png" onclick="setPreviousCurrent()" />
    // <img id="imgCurr" src="../../../App_Resources/images/Circle.png" onclick="setCurrent()" />
    // <img id="imgNext" src="../../../App_Resources/images/Right.png" onclick="setNextCurrent()" />

    if (markerPoints[(currentMarker.tree.Number - 2)]) {
        var prevMarker = markerPoints[(currentMarker.tree.Number - 2)];
        divPrev.innerHTML = prevMarker.tree.Number.toString();
        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        imgPrev.src = apiLocation + "/App_Resources/images/Left" + prevMarker.tree.Code + ".png";
    }
    else if (markerPoints[markerPoints.length - 1]) {
        var prevMarker = markerPoints[markerPoints.length - 1];
        divPrev.innerHTML = prevMarker.tree.Number.toString();
        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        imgPrev.src = apiLocation + "/App_Resources/images/Left" + prevMarker.tree.Code + ".png";
    }
    else
        divPrev.innerHTML = "";

    if (markerPoints[(currentMarker.tree.Number - 1)]) {
        var currMarker = markerPoints[(currentMarker.tree.Number - 1)];
        divCurr.innerHTML = currMarker.tree.Number.toString();
        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        imgCurr.src = apiLocation + "/App_Resources/images/Circle" + currMarker.tree.Code + ".png";
        imgCurr2.src = apiLocation + "/App_Resources/images/Circle" + currMarker.tree.Code + "_B.png";
    }

    if (markerPoints[(currentMarker.tree.Number)]) {
        var nextMarker = markerPoints[(currentMarker.tree.Number)];
        divNext.innerHTML = nextMarker.tree.Number.toString();
        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        imgNext.src = apiLocation + "/App_Resources/images/Right" + nextMarker.tree.Code + ".png";
    }
    else if (markerPoints[0]) {
        var nextMarker = markerPoints[0];
        divNext.innerHTML = nextMarker.tree.Number.toString();
        var apiLocation = document.domain == 'localhost' ? "" : "/web";
        imgNext.src = apiLocation + "/App_Resources/images/Right" + nextMarker.tree.Code + ".png";
    }
    else
        divNext.innerHTML = "";
}

function setNextCurrent() {
    if (markerPoints.length > 0)
        if (markerPoints[(currentMarker.tree.Number)]) {
            SetCurrentMarker(markerPoints[(currentMarker.tree.Number)], true);
        } else if (markerPoints[0]) {// Si no hay despues ve al primero. como en circulo
            SetCurrentMarker(markerPoints[0], true);
        }
}


function setCurrent() {
    if (currentMarker) {
        var isOpen = infoBubble.isOpen_;
        SetCurrentMarker(currentMarker, true);
        if (!isOpen)
            openInfoWindowMarker(currentMarker);
    }
}



function setPreviousCurrent() {
    if (markerPoints.length > 0)
        if (markerPoints[(currentMarker.tree.Number - 2)]) {
            SetCurrentMarker(markerPoints[(currentMarker.tree.Number - 2)], true);
        } else if (markerPoints[(markerPoints.length - 1)]) {// Si no hay anterior ve al ultimo. como en circulo
            SetCurrentMarker(markerPoints[(markerPoints.length - 1)], true);
        }
}


function getMarker(_number) {
    var returnMarker;
    for (var i = 0; i < markerPoints.length; i++) {
        if (markerPoints[i].tree.Number == _number)
            returnMarker = markerPoints[i];
    }
    return returnMarker;
}




//function setDeleteMarker(projectOrganismID) {
//    _projectOrganismID = currentMarker.tree.ProjectOrganismID;
//    for (var i = (markers.length - 1) ; i >= 0; i--) {
//        if (_projectOrganismID == markers[i].tree.ProjectOrganismID) {
//            markers[i].setMap(null);
//            markers.splice(i, 1);
//        }
//    }
//}




//function InitializeMarkers(_markers) {
//    // var xax = '0';
//    // try {
//    for (var i = 0; i < markers.length; i++) {
//        markers[i].setMap(null);
//        // xax = '0' + " - " + i;
//    }
//    markers = [];
//    currentMarker = null;
//    zIndex = 0;
//    // xax = '1';
//    for (var i = 0; i < _markers.length; i++) {
//        var marker = _markers[i];
//        zIndex = (zIndex + 1);
//        // xax = '2';
//        var markerLatLng = new google.maps.LatLng(marker.Lat, marker.Lon);


//        if (marker.CommonNameID == "356") {
//            marker.Code = "DAA8DD";
//        }
//        // xax = '3 ' + " - " + i;
//        var newMarker = new google.maps.Marker({
//            icon: "https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|" + marker.Code + "|11|_|" + marker.Number.toString(),
//            draggable: false,
//            position: markerLatLng,
//            //animation : google.maps.Animation.DROP,
//            map: mapMarker,
//            title: marker.Number.toString(),
//            order: i,
//            zIndex: zIndex,
//            tree: marker
//        });
//        // xax = '4' + " - " + i;
//        markers.push(newMarker);
//        // xax = '5' + " - " + i;
//        AddEventsToMarker(newMarker);
//        // xax = '6' + " - " + i;
//        newMarker = null;
//    }


//    // xax = '7' + " - " + i;
//    if (markers.length > 0) {
//        // xax = '8' + " - " + i;
//        SetCurrentMarker(markers[markers.length - 1]);
//        // xax = '9' + " - " + i;
//    }
//    // } catch(err) {
//    // alert(xax + err);
//    // }
//}


//function getMarker(_number) {
//    var returnMarker;
//    for (var i = 0; i < markers.length; i++) {
//        if (markers[i].tree.Number == _number)
//            returnMarker = markers[i];
//    }
//    return returnMarker;
//}


//function SetCurrentMarker(_marker, _centerIt) {
//    zIndex = (zIndex + 1);


//    if (currentMarker) {
//        // Quitar el texto en BOLD al viejo
//        currentMarker.setIcon("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.85|0|" + currentMarker.tree.Code + "|11|_|" + currentMarker.tree.Number.toString());
//        //
//        currentMarker.draggable = false;
//    }
//    currentMarker = _marker;
//    currentMarker.draggable = true;
//    currentMarker.setZIndex(zIndex);
//    // Poner el texto en BOLD al nuevo
//    currentMarker.setIcon("https://chart.googleapis.com/chart?chst=d_map_spin&chld=0.90|0|" + currentMarker.tree.Code + "|13|b|" + currentMarker.tree.Number.toString());
//    //
//    if (_centerIt) {
//        var latlng = new google.maps.LatLng(currentMarker.getPosition().lat(), currentMarker.getPosition().lng());
//        mapMarker.setCenter(latlng);
//    }
//    Ti.App.fireEvent('app:setCenterText', {
//        Number: currentMarker.tree.Number.toString(),
//        Color: currentMarker.tree.Code
//    });


//}


//function AddEventsToMarker(_newMarker) {
//    google.maps.event.addListener(_newMarker, 'dragend', function () {
//        _newMarker.tree.Lat = this.getPosition().lat();
//        _newMarker.tree.Lon = this.getPosition().lng();
//        Ti.App.fireEvent('app:moveMarker', _newMarker.tree);
//        SetCurrentMarker(_newMarker, false);
//    });


//    google.maps.event.addListener(_newMarker, 'click', function () {
//        SetCurrentMarker(_newMarker, false);
//    });


//    google.maps.event.addListener(_newMarker, 'dblclick', function () {
//        SetCurrentMarker(_newMarker, false);
//        Ti.App.fireEvent('app:openMenu', _newMarker);
//    });


//}


//function InitiateVariables() {
//    polygons = [];
//    markers = [];
//    currentPolyline = null;
//    currentMarker = null;
//    flightPlanCoordinates = [];
//    drawingManager = null;
//    zIndex = 0;
//}



//Ti.App.addEventListener('pageReady', function (e) {
//    InitiateVariables();
//    var project = e.project;
//    var latlng = new google.maps.LatLng(project.Lat, project.Lon);
//    var myOptions = {
//        zoom: 18,
//        center: latlng,
//        mapTypeId: google.maps.MapTypeId.HYBRID,
//        mapTypeControl: true,
//        mapTypeControlOptions: {
//            style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
//            mapTypeIds: [google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.SATELLITE, google.maps.MapTypeId.TERRAIN, google.maps.MapTypeId.HYBRID]
//        },
//        zoomControl: true,
//        zoomControlOptions: {
//            style: google.maps.ZoomControlStyle.LARGE
//        },
//        panControl: false,
//        scaleControl: true,
//        streetViewControl: false,
//        overviewMapControl: false,
//        rotateControl: false
//    };


//    mapMarker = new google.maps.Map(document.getElementById("map_canvas"), myOptions);


//    //Drawing Manager
//    drawingManager = new google.maps.drawing.DrawingManager({
//        drawingMode: google.maps.drawing.OverlayType.MARKER,
//        drawingControl: true,
//        drawingControlOptions: {
//            position: google.maps.ControlPosition.TOP_LEFT,
//            drawingModes: [google.maps.drawing.OverlayType.MARKER]
//        },
//        markerOptions: {
//            draggable: true
//        }
//    });


//    google.maps.event.addListener(drawingManager, 'markercomplete', function (_markerTemp) {
//        Ti.App.fireEvent('app:addMarkerFromControl', {
//            Lat: _markerTemp.getPosition().lat(),
//            Lon: _markerTemp.getPosition().lng()
//        });
//        _markerTemp.setMap(null);
//        drawingManager.setDrawingMode(null);
//    });


//    drawingManager.markerOptions.map = mapMarker;
//    drawingManager.setMap(mapMarker);
//    drawingManager.setDrawingMode(null);
//    //


//    Ti.App.fireEvent('app:pageReadyCompleted', project);
//});


//Ti.App.addEventListener('InitializePerimeters', function (e) {
//    InitializePerimeters(e.Perimeters);
//});


//Ti.App.addEventListener('InitializeMarkers', function (e) {
//    InitializeMarkers(e.Trees);
//});