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
exports.ExternalFlightsComponent = void 0;
var core_1 = require("@angular/core");
var ExternalFlightsComponent = /** @class */ (function () {
    function ExternalFlightsComponent(baseUrl, http, eventEmitterService) {
        var _this = this;
        this.eventEmitterService = eventEmitterService;
        this.extendedFlights = [];
        if (this.eventEmitterService.subsExternal == undefined) {
            this.eventEmitterService.subsExternal = this.eventEmitterService.
                invokeExternalFlightComponentFunction.subscribe(function (params) {
                _this.clean();
            });
        }
    }
    ExternalFlightsComponent.prototype.flightPlanLoadDetailClick = function (serverURL, flightId) {
        this.selected_flight_id = flightId;
        this.eventEmitterService.onClickLoadFlightDetails([serverURL, flightId]);
        this.eventEmitterService.onClickInternalClean();
    };
    ExternalFlightsComponent.prototype.clean = function () {
        this.selected_flight_id = '';
    };
    __decorate([
        core_1.Input()
    ], ExternalFlightsComponent.prototype, "extendedFlights", void 0);
    __decorate([
        core_1.Input()
    ], ExternalFlightsComponent.prototype, "selected_flight_id", void 0);
    ExternalFlightsComponent = __decorate([
        core_1.Component({
            selector: 'external-flights',
            templateUrl: './external-flights.component.html',
            styleUrls: ['./external-flights.component.css']
        }),
        __param(0, core_1.Inject('BASE_URL'))
    ], ExternalFlightsComponent);
    return ExternalFlightsComponent;
}());
exports.ExternalFlightsComponent = ExternalFlightsComponent;
//# sourceMappingURL=external-flights.component.js.map