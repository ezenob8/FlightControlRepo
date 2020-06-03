"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.AppModule = exports.getBaseUrl = void 0;
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/common/http");
var core_2 = require("@agm/core");
var ngx_file_drop_1 = require("ngx-file-drop");
var event_emitter_service_1 = require("./event-emitter.service");
var app_component_1 = require("./app.component");
var flight_details_component_1 = require("./flight-details/flight-details.component");
var my_flights_component_1 = require("./my-flights/my-flights.component");
var external_flights_component_1 = require("./external-flights/external-flights.component");
var toggle_button_component_1 = require("./toggle-button.component");
function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
exports.getBaseUrl = getBaseUrl;
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [
                platform_browser_1.BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
                forms_1.FormsModule,
                http_1.HttpClientModule,
                core_2.AgmCoreModule.forRoot({
                    // please get your own API key here:
                    // https://developers.google.com/maps/documentation/javascript/get-api-key?hl=en
                    apiKey: 'AIzaSyC0cZhn4jFA7YotLo4msJAyprA6xE0tunk'
                }),
                ngx_file_drop_1.NgxFileDropModule
            ],
            declarations: [app_component_1.AppComponent, flight_details_component_1.FlightDetailsComponent, my_flights_component_1.MyFlightsComponent, external_flights_component_1.ExternalFlightsComponent, toggle_button_component_1.ToggleButtonComponent],
            providers: [
                { provide: 'BASE_URL', useFactory: getBaseUrl },
                { provide: core_2.FitBoundsAccessor, useExisting: core_1.forwardRef(function () { return core_2.AgmMarker; }) },
                event_emitter_service_1.EventEmitterService
            ],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map