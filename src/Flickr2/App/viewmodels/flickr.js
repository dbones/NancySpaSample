define(['plugins/http', 'durandal/app', 'knockout'], function (http, app, ko) {
    //Note: This module exports an object.
    //That means that every module that "requires" it will get the same object instance.
    //If you wish to be able to create multiple instances, instead export a function.

    return {
        searchTag : ko.observable(""),
        search: function () {
            var that = this;
            return http.jsonp('http://api.flickr.com/services/feeds/photos_public.gne', { tags: that.searchTag(), tagmode: 'any', format: 'json' }, 'jsoncallback').then(function(response) {
                that.images(response.items);
            });
        },
        displayName: 'Flickr',
        images: ko.observableArray([]),
        activate: function () { this.search(); },
        select: function(item) {
            //AJAX :)
            http.post('/Images', { id: item.link });
        },
    };
});