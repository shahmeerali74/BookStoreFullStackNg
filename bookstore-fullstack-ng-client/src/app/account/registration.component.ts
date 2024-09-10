import { ChangeDetectionStrategy, Component } from "@angular/core";
import { RegistrationFormComponet } from "./ui/registration-form.component";

@Component({
  selector: "app-registration",
  standalone: true,
  imports: [RegistrationFormComponet],
  template: ` <app-registration-form /> `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegistrationComponent {}
