import { ChangeDetectionStrategy, Component } from "@angular/core";
import { RegistrationFormComponet } from "./ui/registration-form.component";
import { RegistrationModel } from "./data/registration.model";

@Component({
  selector: "app-registration",
  standalone: true,
  imports: [RegistrationFormComponet],
  template: ` <app-registration-form (submitData)="onFormSubmit($event)" /> `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegistrationComponent {
  onFormSubmit(registrationData: RegistrationModel) {
    console.log(registrationData);
  }
}
