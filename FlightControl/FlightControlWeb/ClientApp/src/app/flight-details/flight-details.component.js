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
exports.FlightDetailsComponent = void 0;
var core_1 = require("@angular/core");
var FlightDetailsComponent = /** @class */ (function () {
    function FlightDetailsComponent(http, baseUrl, eventEmitterService) {
        this.eventEmitterService = eventEmitterService;
        this.baseUrl = baseUrl;
        this.http = http;
    }
    FlightDetailsComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.eventEmitterService.subsFlightPlan == undefined) {
            this.eventEmitterService.subsFlightPlan = this.eventEmitterService.
                invokeFlightPlanComponentFunction.subscribe(function (params) {
                _this.loadDetails(params);
            });
        }
    };
    FlightDetailsComponent.prototype.loadDetails = function (params) {
        var _this = this;
        if (params[1] == 'clean') {
            this.selectedFlightPlan = null;
            this.eventEmitterService.invokeAppComponentFunction.emit(params);
        }
        else {
            this.http.get(params[0] + 'api/FlightPlan' + '/' + params[1]).subscribe(function (result) {
                var sum = 0;
                result.segments.forEach(function (segment, index) {
                    return sum += segment.timespan_seconds;
                });
                var ini_date = new Date(result.initial_location.date_time);
                var end_date = new Date(result.initial_location.date_time);
                end_date.setSeconds(end_date.getSeconds() + sum);
                _this.selected_ini_date_flight = ini_date;
                _this.selected_end_date_flight = end_date;
                _this.selected_final_location = result.segments[result.segments.length - 1];
                _this.selectedFlightPlan = result;
            }, function (error) { return console.error(error); });
            this.eventEmitterService.invokeAppComponentFunction.emit(params);
        }
    };
    FlightDetailsComponent = __decorate([
        core_1.Component({
            selector: 'flight-details',
            templateUrl: './flight-details.component.html'
        }),
        __param(1, core_1.Inject('BASE_URL'))
    ], FlightDetailsComponent);
    return FlightDetailsComponent;
}());
exports.FlightDetailsComponent = FlightDetailsComponent;
//# sourceMappingURL=flight-details.component.js.map