function NFileAPI() {}
(function($){
    $.getInstantiator = function () {
        if(!$.hostingEl){
            $.hostingEl = document.createElement('span');
            $.hostingEl.innerHTML = Silverlight.createObjectEx({
                source:'../Bin/Debug/NFileAPI.xap',
                properties: {
                    version: '3.0',
                    autoUpgrade: 'true',
                    height: '23px',
                    width: '56px'
                } ,
                events: {
                    'onLoad': function(){}
                }
            });
            document.getElementsByTagName('body')[0].appendChild($.hostingEl);
            $.waitForSL();
        }
        return $.hostingEl.firstChild;
    };

    $.isSLLoaded = function (){
        var t = typeof $.getInstantiator().Content.services;
        console.log('createObject type is '+t);
        return t != 'undefined';
    };
    
    $.createObject = function(name){
        try{
            return $.getInstantiator().Content.services.createObject(name);
        }catch(e){
            throw new ReferenceError('Silverlight thingy has not been loaded yet');
        }
    };

    $.waitForSL = function(){
        //console.log('Checking for SL...');
        if(!$.isSLLoaded()){
            //console.log('Not loaded, waiting...')
            setTimeout($.waitForSL,100);
            return;
        }
        window.nfaFileReader = function () {
            return $.createObject('FileReader');
        };
        window.nfaBlobBuilder = function(){
            return $.createObject('BlobBuilder');
        };
    };
    
    $.init = function () {
        $.getInstantiator();
    };

})(NFileAPI);
window.addEventListener('load',function(){
    NFileAPI.init();
});


