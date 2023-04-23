/*let xmlContent = '';
let list = document.getElementsByTagName('Placemark');

fetch('assets/allMap.kml').then((response)=>{
    response.text().then((xml)=>{
        let parser = new DOMParser();
        let xmlDOM = parser.parseFromString(xmlContent, 'application/xml');
    });
});

document.querySelector('#elastic').oninput = function() {
    let val = this.value.trim();
    
    if(val != '') {
        list.forEach(function (elem) {
            if(elem.classList.search(val) == -1){
                elem.classList.add('hide');
            }
            else {
                elem.classList.remove('hide');
        }
        });
    }
    else {
        list.forEach(function (elem){
            elem.classList.remove('hide');
        });
    }              
}*/
