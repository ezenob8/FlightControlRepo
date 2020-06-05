"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.AppComponent = void 0;
var http_1 = require("@angular/common/http");
var core_1 = require("@angular/core");
var rxjs_1 = require("rxjs");
var AppComponent = /** @class */ (function () {
    function AppComponent(http, mapsAPILoader, baseUrl, eventEmitterService) {
        var _this = this;
        this.http = http;
        this.mapsAPILoader = mapsAPILoader;
        this.baseUrl = baseUrl;
        this.eventEmitterService = eventEmitterService;
        this.internalFlights = [];
        this.externalFlights = [];
        this.extendedFlights = [];
        this.servers = [];
        this.showLine = false;
        this.icon = {
            url: 'assets/images/plane-rojo.png',
            scaledSize: {
                width: 30,
                height: 30
            }
        };
        this.iconBlack = {
            url: 'assets/images/plane-black.png',
            scaledSize: {
                width: 30,
                height: 30
            }
        };
        //From here drag and drop
        this.files = [];
        this.mapsAPILoader.load().then(function () {
            _this.bounds = new google.maps.LatLngBounds(new google.maps.LatLng(51.130739, -0.868052), // SW
            new google.maps.LatLng(51.891257, 0.559417) // NE
            );
            console.log(_this.bounds);
        });
        var self = this;
        var observer = rxjs_1.interval(3000)
            .subscribe(function (val) {
            //Internal Flights
            http.get(baseUrl + 'api/flights/activeinternalflights').subscribe(function (resultInternal) {
                self.internalFlights = resultInternal;
            });
            //External Flights
            http.get(baseUrl + 'api/servers').subscribe(function (resultServer) {
                self.servers = resultServer;
                self.servers.forEach(function (server) {
                    //TODO: agregar + 'api/flights/relative_to=' + new Date().toISOString()
                    //console.log(new Date().toISOString());
                    http.get(self.servers[0].serverURL + 'api/Flights?relative_to=' + new Date().toISOString().substring(0, 19) + 'Z').subscribe(function (resultExternal) {
                        var ext = [];
                        ;
                        self.externalFlights = resultExternal;
                        self.externalFlights.forEach(function (item) {
                            var extendedFlight = {
                                flight_id: item.flight_id,
                                longitude: item.longitude,
                                latitude: item.latitude,
                                passengers: item.passengers,
                                company_name: item.company_name,
                                date_time: item.date_time,
                                is_external: item.is_external,
                                serverId: server.serverId,
                                serverURL: server.serverURL
                            };
                            ext.push(extendedFlight);
                            self.extendedFlights = ext;
                        });
                    }, function (error) { return console.error(error); });
                });
            }, function (error) { return console.error(error); });
        });
    }
    AppComponent.prototype.mapReady = function (map) {
        map.fitBounds(this.bounds);
    };
    AppComponent.prototype.dropped = function (files) {
        var _this = this;
        this.files = files;
        var _loop_1 = function (droppedFile) {
            // Is it a file?
            if (droppedFile.fileEntry.isFile) {
                var fileEntry = droppedFile.fileEntry;
                fileEntry.file(function (file) {
                    // Here you can access the real file
                    console.log(droppedFile.relativePath, file);
                    var fileReader = new FileReader();
                    var self = _this;
                    fileReader.onloadend = function (x) {
                        console.log("jsondata:" + fileReader.result.toString());
                        self.postData(fileReader.result.toString());
                    };
                    fileReader.readAsText(file);
                });
            }
            else {
                // It was a directory (empty directories are added, otherwise only files)
                var fileEntry = droppedFile.fileEntry;
                console.log(droppedFile.relativePath, fileEntry);
            }
        };
        for (var _i = 0, files_1 = files; _i < files_1.length; _i++) {
            var droppedFile = files_1[_i];
            _loop_1(droppedFile);
        }
    };
    AppComponent.prototype.fileOver = function (event) {
        console.log(event);
    };
    AppComponent.prototype.fileLeave = function (event) {
        console.log(event);
    };
    AppComponent.prototype.postData = function (jsondata) {
        //// Headers
        var headers = new http_1.HttpHeaders({
            'security-token': 'mytoken'
        });
        console.log(this.baseUrl);
        this.http.post(this.baseUrl + 'api/FlightPlan', JSON.parse(jsondata), { headers: headers, responseType: 'json' })
            .subscribe(function (data) {
        });
    };
    AppComponent.prototype.flightPlanLoadDetailClick = function (serverURL, flightId) {
        if (serverURL == '')
            serverURL = this.baseUrl;
        this.eventEmitterService.onClickLoadFlightDetails([serverURL, flightId]);
        if (serverURL == 'clean') {
        }
        else {
            //this.http.get<FlightPlanDTO>(serverURL + 'api/FlightPlan' + '/' + flightId).subscribe(result => {
            //  this.selectedFlightPlan = result;
            //  console.log(this.selectedFlightPlan);
            //}, error => console.error(error), () => this.showLine = true);
            console.log(serverURL);
            this.selectedFlightPlan$ = this.http.get(serverURL + 'api/FlightPlan' + '/' + flightId);
        }
    };
    AppComponent.prototype.clean = function () {
        this.eventEmitterService.onClickLoadFlightDetails(['', 'clean']);
    };
    AppComponent = __decorate([
        core_1.Component({
            selector: "app-map",
            templateUrl: "./app.component.html",
            styleUrls: ["./app.component.css"]
        }),
        __param(2, core_1.Inject('BASE_URL'))
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map