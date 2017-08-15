// Ensure root directory has trailing slash
// currently we're not able to do this redirect in the StaticFileServer because the proxy strips off the trailing slash
(function () {
    var loc = window.location;
    if (loc.pathname.match(/\/[^\/\.]+$/)) {
        window.location.href = loc.pathname + '/' + loc.search + loc.hash;
    }
}());
