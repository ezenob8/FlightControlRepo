import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'toggle-button',
  template: `
  <div class="custom-control custom-switch m-2">
  <input type="checkbox" class="custom-control-input" id="customSwitch1"  (change)="changed.emit($event.target.checked)" [checked]="showDrop">
  <label class="custom-control-label" for="customSwitch1">Enable/Disable Drop File</label>
</div>
  `,
  styles: [`
    
  `]
})
export class ToggleButtonComponent {
  @Output() changed = new EventEmitter<boolean>();
}
