"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.EventEmitterService = void 0;
var core_1 = require("@angular/core");
var EventEmitterService = /** @class */ (function () {
    function EventEmitterService() {
        this.invokeFlightPlanComponentFunction = new core_1.EventEmitter(false);
        this.invokeAppComponentFunction = new core_1.EventEmitter(false);
        this.invokeInternalFlightComponentFunction = new core_1.EventEmitter(false);
        this.invokeExternalFlightComponentFunction = new core_1.EventEmitter(false);
    }
    EventEmitterService.prototype.onClickLoadFlightDetails = function (params) {
        this.invokeFlightPlanComponentFunction.emit(params);
    };
    EventEmitterService.prototype.onClickShowPoligone = function (params) {
        this.invokeAppComponentFunction.emit(params);
    };
    EventEmitterService.prototype.onClickInternalClean = function () {
        this.invokeInternalFlightComponentFunction.emit();
    };
    EventEmitterService.prototype.onClickExternalClean = function () {
        this.invokeExternalFlightComponentFunction.emit();
    };
    __decorate([
        core_1.Output()
    ], EventEmitterService.prototype, "invokeFlightPlanComponentFunction", void 0);
    __decorate([
        core_1.Output()
    ], EventEmitterService.prototype, "invokeAppComponentFunction", void 0);
    __decorate([
        core_1.Output()
    ], EventEmitterService.prototype, "invokeInternalFlightComponentFunction", void 0);
    __decorate([
        core_1.Output()
    ], EventEmitterService.prototype, "invokeExternalFlightComponentFunction", void 0);
    EventEmitterService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        })
    ], EventEmitterService);
    return EventEmitterService;
}());
exports.EventEmitterService = EventEmitterService;
//# sourceMappingURL=event-emitter.service.js.map