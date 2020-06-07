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
exports.MyFlightsComponent = void 0;
var core_1 = require("@angular/core");
var MyFlightsComponent = /** @class */ (function () {
    function MyFlightsComponent(http, baseUrl, eventEmitterService) {
        var _this = this;
        this.http = http;
        this.baseUrl = baseUrl;
        this.eventEmitterService = eventEmitterService;
        if (this.eventEmitterService.subsInternal == undefined) {
            this.eventEmitterService.subsInternal = this.eventEmitterService.
                invokeInternalFlightComponentFunction.subscribe(function (params) {
                _this.clean();
            });
        }
    }
    MyFlightsComponent.prototype.flightPlanLoadDetailClick = function (serverId, flightId) {
        this.selected_flight_id = flightId;
        this.eventEmitterService.onClickLoadFlightDetails([this.baseUrl, flightId]);
        this.eventEmitterService.onClickExternalClean();
    };
    MyFlightsComponent.prototype.delete = function (flightId) {
        console.log('aaa' + flightId);
        this.http.delete(this.baseUrl + 'api/flights/' + flightId).subscribe(function (result) {
        }, function (error) { return console.error(error); });
        this.eventEmitterService.onClickLoadFlightDetails([this.baseUrl, 'clean']);
    };
    MyFlightsComponent.prototype.clean = function () {
        this.selected_flight_id = '';
    };
    __decorate([
        core_1.Input()
    ], MyFlightsComponent.prototype, "flights", void 0);
    MyFlightsComponent = __decorate([
        core_1.Component({
            selector: 'my-flights',
            templateUrl: './my-flights.component.html',
            styleUrls: ['./my-flights.component.css']
        }),
        __param(1, core_1.Inject('BASE_URL'))
    ], MyFlightsComponent);
    return MyFlightsComponent;
}());
exports.MyFlightsComponent = MyFlightsComponent;
//# sourceMappingURL=my-flights.component.js.map