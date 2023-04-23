const mymap = L.map('mapid').setView([67, 83], 4);
mymap.zoomControl.setPosition('topright');

let dlg_layer = L.layerGroup();
let sel_layer = L.layerGroup();
let xas_layer = L.layerGroup();
let path_dlg = L.layerGroup();
let path_sel = L.layerGroup();
let path_xas = L.layerGroup();


L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw', {
minZoom: 3,
maxZoom: 20,
attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, ' +
'<a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
id: 'mapbox/streets-v11',
tileSize: 512,
zoomOffset: -1
}).addTo(mymap);

/*fetch('assets/INEL_LangsRecolored.kml')
    .then(res => res.text())
    .then(kmltext => {
        // Create new kml overlay
        const parser = new DOMParser();
        const kml = parser.parseFromString(kmltext, 'text/xml');
        const track = new L.KML(kml);
        mymap.addLayer(track);

        // Adjust map to show the kml
        const bounds = track.getBounds();
        mymap.fitBounds(bounds);
    });*/

let m_layer = L.layerGroup();
let f_layer = L.layerGroup();
let unknown_year = L.layerGroup();

function output_dlg(num){
    dlg_layer.clearLayers();
    let parser, xmlDoc;
    var layer = [], l, path = [];
    let check = document.getElementById('check_switch');
    let checkbox_lang = document.getElementById("lang1");
    let checkbox_male = document.getElementById("sex1");
    let checkbox_female = document.getElementById("sex2");
    let checkbox_unknown = document.getElementById('agenone');

    if(checkbox_lang.checked){
        fetch('assets/allKML.kml')
        .then(res => res.text())
        .then(kmltext => {
            parser = new DOMParser();
            xmlDoc = parser.parseFromString(kmltext, 'text/xml');

            var style = L.KML.parseStyles(xmlDoc);
            L.KML.parseStyleMap(xmlDoc, style);

            let list_points = xmlDoc.getElementsByTagName('Placemark');

            for(let i = 0; i < list_points.length; i++){
                let help = list_points[i].getElementsByTagName('Data')[0].childNodes[1].childNodes[0].nodeValue;
                if(help != 'Path'){
                    if(help == "Text"){
                        let year_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                        if(year_help != '...' && list_points[i].getElementsByTagName('Data')[5].childNodes[1].childNodes[0].nodeValue == 'dlg'){
                            if(+year_help > +sliders[0].value && +year_help < +sliders[1].value){
                                l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                if(l) { layer.push(l); }
                            }
                        }
                    }
                    else{
                        if(help == "From Text"){
                            let year_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                            if(year_help != '...' && list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue == 'dlg'){
                                if(+year_help > +sliders[0].value && +year_help < +sliders[1].value){
                                    l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                    if(l) { layer.push(l); }
                                }   
                            }
                        }

                        if(help == 'Place of birth' || help == 'Domicile' || help == 'Former residence'){
                        
                            if(checkbox_male.checked){
                                let year_help = list_points[i].getElementsByTagName('Data')[7].childNodes[1].childNodes[0].nodeValue;
                                lang_help = list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue;
                                sex_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                                if( sex_help == 'male' && lang_help == 'dlg'){
                                    if(year_help != '...' && +year_help > +sliders[2].value && +year_help < +sliders[3].value)
                                    {
                                        l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                        if(l) { layer.push(l); }
                                    }
                                    else{
                                        if(checkbox_unknown.checked){
                                            l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                            if(l) { layer.push(l); }
                                            
                                            for(let i = 0; i < layer.length; i++){
                                                unknown_year.addLayer(layer[i]);
                                            }
                                            unknown_year.addTo(mymap); 
                                        }
                                        else{
                                            unknown_year.clearLayers();
                                        }
                                    }
                                }
                                for(let i = 0; i < layer.length; i++){
                                    m_layer.addLayer(layer[i]);
                                }
                                m_layer.addTo(mymap);
                            }
                            else{
                                m_layer.clearLayers();
                            }  
                            
                            if(checkbox_female.checked){
                                let year_help = list_points[i].getElementsByTagName('Data')[7].childNodes[1].childNodes[0].nodeValue;
                                lang_help = list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue;
                                sex_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                                if( sex_help == 'female' && lang_help == 'dlg'){
                                    if(year_help != '...' && +year_help > +sliders[2].value && +year_help < +sliders[3].value)
                                    {
                                        l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                        if(l) { layer.push(l); }
                                    }
                                    else{
                                        if(checkbox_unknown.checked){
                                            l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                            if(l) { layer.push(l); }
                                            
                                            for(let i = 0; i < layer.length; i++){
                                                unknown_year.addLayer(layer[i]);
                                            }
                                            unknown_year.addTo(mymap); 
                                        }
                                        else{
                                            unknown_year.clearLayers();
                                        }
                                    }
                                }
                                for(let i = 0; i < layer.length; i++){
                                    f_layer.addLayer(layer[i]);
                                }
                                f_layer.addTo(mymap);
                            }
                            else{
                                f_layer.clearLayers();
                            }
                        }
                    }
                }
                else{
                    if(check.checked){
                        if(list_points[i].getElementsByTagName('styleUrl')[0].childNodes[0].nodeValue == '#dlg'){
                            l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                            if(l) { path.push(l); }
                        }
                        for(let i = 0; i < path.length; i++){
                            path_dlg.addLayer(path[i]);
                        }
                        path_dlg.addTo(mymap);
                    }
                    else{
                        path_dlg.clearLayers();
                    }
                }
            };
            for(let i = 0; i < layer.length; i++){
                dlg_layer.addLayer(layer[i]);
            }
            dlg_layer.addTo(mymap);
        });
    }
    else{
        dlg_layer.clearLayers();
        path_dlg.clearLayers();
    }
}

function output_xas(num){
    xas_layer.clearLayers();
    let parser, xmlDoc;
    var layer = [], l, path = [];
    let check = document.getElementById('check_switch');
    let checkbox_lang = document.getElementById("lang3");
    let checkbox_male = document.getElementById("sex1");
    let checkbox_female = document.getElementById("sex2");
    let checkbox_unknown = document.getElementById("agenone");

    if(checkbox_lang.checked){
        fetch('assets/allKML.kml')
        .then(res => res.text())
        .then(kmltext => {
            parser = new DOMParser();
            xmlDoc = parser.parseFromString(kmltext, 'text/xml');

            var style = L.KML.parseStyles(xmlDoc);
            L.KML.parseStyleMap(xmlDoc, style);

                let list_points = xmlDoc.getElementsByTagName('Placemark');

                for(let i = 0; i < list_points.length; i++){
                    let help = list_points[i].getElementsByTagName('Data')[0].childNodes[1].childNodes[0].nodeValue;
                    if(help != 'Path'){
                        if(help == "Text"){
                            let year_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                            if(year_help != '...' && list_points[i].getElementsByTagName('Data')[5].childNodes[1].childNodes[0].nodeValue == 'xas'){
                                if(+year_help > +sliders[0].value && +year_help < +sliders[1].value){
                                    l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                    if(l) { layer.push(l); }
                                }
                            }
                        }
                        else{
                            if(help == "From Text"){
                                let year_help = list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue;
                                if(year_help != '...' && list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue == 'xas'){
                                    if(+year_help > +sliders[0].value && +year_help < +sliders[1].value){
                                        l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                        if(l) { layer.push(l); }
                                    }   
                                }
                            }
                            
                            if(help == 'Place of birth' || help == 'Domicile' || help == 'Former residence'){
                        
                                if(checkbox_male.checked){
                                    let year_help = list_points[i].getElementsByTagName('Data')[7].childNodes[1].childNodes[0].nodeValue;
                                        lang_help = list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue;
                                        sex_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                                        if( sex_help == 'male' && lang_help == 'xas'){
                                            if(year_help != '...' && +year_help > +sliders[2].value && +year_help < +sliders[3].value)
                                            {
                                                l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                                if(l) { layer.push(l); }
                                            }
                                            else{
                                                if(checkbox_unknown.checked){
                                                    l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                                    if(l) { layer.push(l); }
                                                    
                                                    for(let i = 0; i < layer.length; i++){
                                                        unknown_year.addLayer(layer[i]);
                                                    }
                                                    unknown_year.addTo(mymap); 
                                                }
                                                else{
                                                    unknown_year.clearLayers();
                                                }
                                            }
                                        }
                                        
                                    for(let i = 0; i < layer.length; i++){
                                        m_layer.addLayer(layer[i]);
                                    }
                                    m_layer.addTo(mymap);
                                }
                                else{
                                    m_layer.clearLayers();
                                }  
                                
                                if(checkbox_female.checked){
                                    let year_help = list_points[i].getElementsByTagName('Data')[7].childNodes[1].childNodes[0].nodeValue;
                                    lang_help = list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue;
                                    sex_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                                    if( sex_help == 'female' && lang_help == 'xas'){
                                        if(year_help != '...' && +year_help > +sliders[2].value && +year_help < +sliders[3].value)
                                        {
                                            l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                            if(l) { layer.push(l); }
                                        }
                                        else{
                                            if(checkbox_unknown.checked){
                                                l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                                if(l) { layer.push(l); }
                                                
                                                for(let i = 0; i < layer.length; i++){
                                                    unknown_year.addLayer(layer[i]);
                                                }
                                                unknown_year.addTo(mymap); 
                                            }
                                            else{
                                                unknown_year.clearLayers();
                                            }
                                        }
                                    }
                                    for(let i = 0; i < layer.length; i++){
                                        f_layer.addLayer(layer[i]);
                                    }
                                    f_layer.addTo(mymap);
                                }
                                else{
                                    f_layer.clearLayers();
                                }
                            }
                        }
                    }
                    else{
                        if(check.checked){
                            if(list_points[i].getElementsByTagName('styleUrl')[0].childNodes[0].nodeValue == '#xas'){
                                l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                if(l) { path.push(l); }
                            }
                            for(let i = 0; i < path.length; i++){
                                path_xas.addLayer(path[i]);
                            }
                            path_xas.addTo(mymap);
                        }
                        else{
                            path_xas.clearLayers();
                        }
                    }
                };
                for(let i = 0; i < layer.length; i++){
                    xas_layer.addLayer(layer[i]);
                }
                xas_layer.addTo(mymap);
        });
    }
    else{
        xas_layer.clearLayers();
        path_xas.clearLayers();
    }
}

function output_sel(num){
    sel_layer.clearLayers();
    let parser, xmlDoc;
    var layer = [], l, path = [];
    let check = document.getElementById('check_switch');
    let checkbox_lang = document.getElementById("lang2");
    let checkbox_male = document.getElementById("sex1");
    let checkbox_female = document.getElementById("sex2");
    let checkbox_unknown = document.getElementById("agenone");

    if(checkbox_lang.checked){
        fetch('assets/allKML.kml')
        .then(res => res.text())
        .then(kmltext => {
            parser = new DOMParser();
            xmlDoc = parser.parseFromString(kmltext, 'text/xml');

            var style = L.KML.parseStyles(xmlDoc);
            L.KML.parseStyleMap(xmlDoc, style);

                let list_points = xmlDoc.getElementsByTagName('Placemark');

                for(let i = 0; i < list_points.length; i++){
                    let help = list_points[i].getElementsByTagName('Data')[0].childNodes[1].childNodes[0].nodeValue;
                    if(help != 'Path'){
                        if(help == "Text"){
                            let year_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                            if(year_help != '...' && list_points[i].getElementsByTagName('Data')[5].childNodes[1].childNodes[0].nodeValue == 'sel'){
                                if(+year_help > +sliders[0].value && +year_help < +sliders[1].value){
                                    l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                    if(l) { layer.push(l); }
                                }
                            }
                        }
                        else{
                            if(help == "From Text"){
                                let year_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                                if(year_help != '...' && list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue == 'sel'){
                                    if(+year_help > +sliders[0].value && +year_help < +sliders[1].value){
                                        l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                        if(l) { layer.push(l); }
                                    }   
                                }
                            }
                            
                            if(help == 'Place of birth' || help == 'Domicile' || help == 'Former residence'){
                        
                                if(checkbox_male.checked){
                                    let year_help = list_points[i].getElementsByTagName('Data')[7].childNodes[1].childNodes[0].nodeValue;
                                        lang_help = list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue;
                                        sex_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                                        if( sex_help == 'male' && lang_help == 'sel'){
                                            if(year_help != '...' && +year_help > +sliders[2].value && +year_help < +sliders[3].value)
                                            {
                                                l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                                if(l) { layer.push(l); }
                                            }
                                            else{
                                                if(checkbox_unknown.checked){
                                                    l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                                    if(l) { layer.push(l); }
                                                    
                                                    for(let i = 0; i < layer.length; i++){
                                                        unknown_year.addLayer(layer[i]);
                                                    }
                                                    unknown_year.addTo(mymap); 
                                                }
                                                else{
                                                    unknown_year.clearLayers();
                                                }
                                            }
                                        }
                                    for(let i = 0; i < layer.length; i++){
                                        m_layer.addLayer(layer[i]);
                                    }
                                    m_layer.addTo(mymap);
                                }
                                else{
                                    m_layer.clearLayers();
                                }  
                                
                                if(checkbox_female.checked){
                                    let year_help = list_points[i].getElementsByTagName('Data')[7].childNodes[1].childNodes[0].nodeValue;
                                    lang_help = list_points[i].getElementsByTagName('Data')[3].childNodes[1].childNodes[0].nodeValue;
                                    sex_help = list_points[i].getElementsByTagName('Data')[4].childNodes[1].childNodes[0].nodeValue;
                                    if( sex_help == 'female' && lang_help == 'sel'){
                                        if(year_help != '...' && +year_help > +sliders[2].value && +year_help < +sliders[3].value)
                                        {
                                            l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                            if(l) { layer.push(l); }
                                        }
                                        else{
                                            if(checkbox_unknown.checked){
                                                l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                                if(l) { layer.push(l); }
                                                
                                                for(let i = 0; i < layer.length; i++){
                                                    unknown_year.addLayer(layer[i]);
                                                }
                                                unknown_year.addTo(mymap); 
                                            }
                                            else{
                                                unknown_year.clearLayers();
                                            }
                                        }
                                    }
                                    for(let i = 0; i < layer.length; i++){
                                        f_layer.addLayer(layer[i]);
                                    }
                                    f_layer.addTo(mymap);
                                }
                                else{
                                    f_layer.clearLayers();
                                }
                            }
                        }
                    }
                    else{
                        if(check.checked){
                            if(list_points[i].getElementsByTagName('styleUrl')[0].childNodes[0].nodeValue == '#sel'){
                                l = L.KML.parsePlacemark(list_points[i], xmlDoc, style);
                                if(l) { path.push(l); }
                            }
                            for(let i = 0; i < path.length; i++){
                                path_sel.addLayer(path[i]);
                            }
                            path_sel.addTo(mymap);
                        }
                        else{
                            path_sel.clearLayers();
                        }
                    }
                };
                for(let i = 0; i < layer.length; i++){
                    sel_layer.addLayer(layer[i]);
                }
                sel_layer.addTo(mymap);
        });
    }
    else{
        sel_layer.clearLayers();
        path_sel.clearLayers();
    }
}

function outputUpdateYear1(year_v){
    let output = document.getElementById('year1');
    output.value = year_v;
    output.style.right = year_v;
}

function outputUpdateYear2(year_v){
    let output = document.getElementById('year2');
    output.value = year_v;
    output.style.right = year_v;
}

function outputUpdateAge1(age_v){
    let output = document.getElementById('age1');
    output.value = age_v;
    output.style.right = age_v;
}

function outputUpdateAge2(age_v){
    let output = document.getElementById('age2');
    output.value = age_v;
    output.style.right = age_v;
}

const sliders = document.querySelectorAll('input[type="range"]');

sliders[0].addEventListener('input', (e) => {
 if(+sliders[0].value > +sliders[1].value){
    sliders[1].value = +sliders[0].value;
  }
});

sliders[1].addEventListener('input', (e) => {
 if(+sliders[1].value < +sliders[0].value){
    sliders[0].value = +sliders[1].value;
  }
});

sliders[2].addEventListener('input', (e) => {
    if(+sliders[2].value > +sliders[3].value){
       sliders[3].value = +sliders[2].value;
     }
   });
   
sliders[3].addEventListener('input', (e) => {
if(+sliders[3].value < +sliders[2].value){
    sliders[2].value = +sliders[3].value;
    }
});