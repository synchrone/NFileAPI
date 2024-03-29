function NFileAPI() {}
(function($){
    $.extend = function(destination, source) {
        for (var property in source) {
            if (source.hasOwnProperty(property)) {
                destination[property] = source[property];
            }
        }
        return destination;
    };

    $.config = {
        source: 'nfileapi/NFileAPI.xap',
        onError: function(){},
        onLoad: function(){}
    };

    $.createObj = function(config){
        if(typeof config == 'undefined'){
            config = $.config;
        }
        var hostingEl = document.createElement('span');

        hostingEl.innerHTML = Silverlight.createObjectEx({
            source: $.config.source,
            properties: {
                version: '3.0',
                autoUpgrade: 'true',
                height: '23px',
                width: '56px'
            } ,
            events: {
                'onLoad': function(){},
                'onStartup': function(){},
                'onChange': function(){
                    if(hostingEl.dispatchEvent){
                        var evt = document.createEvent("Event");
                        evt.initEvent('change',false,false);
                        hostingEl.dispatchEvent(evt);

                    }else if(hostingEl.fireEvent){
                        hostingEl.fireEvent('onchange');
                    }else if(hostingEl.onchange){
                        hostingEl.onchange.call(hostingEl);
                    }
                }
            }
        });
        
        Object.defineProperty(hostingEl,'files', {
            //writable: false,
            //enumerable: true,
            get: function(){ //this is solely for the .files[index] purpose. Could've just returned files if not that
                var files = [];
                try{
                    for (var i = 0, file; file = hostingEl.childNodes[0].Content.input.files.item(i); ++i){
                        files.push(file);
                    }
                }catch(e){}
                return files;
            }
        });
        

        return hostingEl;
    };
    
    $.getInstantiator = function () {
        if(!$.hostingEl){
            $.hostingEl = $.createObj();
            $.hostingEl.style.position = 'absolute';
            $.hostingEl.style.visibility = 'hidden';
            document.getElementsByTagName('body')[0].appendChild($.hostingEl);
            $.waitForSL();
        }
        return $.hostingEl.firstChild;
    };

    $.isSLLoaded = function (){
        try{
            var t = typeof $.getInstantiator().Content.services.createObject;
            //console.log('createObject type is '+t);
            return t != 'undefined';
        }catch(e){
            //console.log('Exception checking for createObject avail... '+e)
            return false;
        }
    };
    
    $.createObject = function(name,instantiator){
        instantiator = instantiator || $.getInstantiator();
        try{
            return instantiator.Content.services.createObject(name);
        }catch(e){
            throw new ReferenceError('Silverlight thingy has not been loaded yet');
        }
    };

    $.waitForSL = function(){
        //console.log('Checking for SL...');
        if(!$.isSLLoaded()){
            //console.log('Not loaded, waiting...');
            setTimeout($.waitForSL,100);
            return;
        }
        window.nfaFileReader = function () {
            return $.createObject('FileReader');
        };
        
        window.nfaBlobBuilder = function(){};
        (function(fr){
            fr.readAsArrayBuffer = function(blob){
                fr.prototype = $.createObject('FileReader',blob.HostingElement);
                for(var i = 0, ev; ev = fr.eventList; i++){
                    fr.prototype[ev] = fr[ev];
                }
            };
            fr.eventList = ['onloadstart', 'onprogress', 'onabort', 'onload', 'onloadend'];
        })(window.nfaFileReader);
    };
    
    $.init = function (config) {
        $.config = $.extend($.config,config);

        $.getInstantiator();

        var elements = document.getElementsByTagName('input');
        (function(elements)
        {
            for (var i = 0, el; el = elements[i]; ++i)
            {
                if(el.getAttribute('type')!='file'){
                    continue;
                }
                var slTag = $.createObj();
                for (var i = 0, attr; attr = el.attributes[i]; ++i){
                    slTag.setAttribute(attr.nodeName, attr.nodeValue);
                }
                el.parentNode.replaceChild(slTag,el);
            }
        })(elements);
    };

})(NFileAPI);


