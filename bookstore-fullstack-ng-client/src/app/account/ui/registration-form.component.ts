import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RegistrationModel } from "../data/registration.model";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatButtonModule } from "@angular/material/button";
import { MatInputModule } from "@angular/material/input";

@Component({
  selector: "app-registration-form",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
  ],
  template: `
    <div class="registration-form-container">
      <h2>Signup</h2>
      <form [formGroup]="registrationForm" (ngSubmit)="submit()">
        <mat-form-field appearance="fill">
          <mat-label>Name</mat-label>
          <input matInput formControlName="name" />
        </mat-form-field>

        <mat-form-field appearance="fill">
          <mat-label>Email</mat-label>
          <input matInput formControlName="email" />
        </mat-form-field>

        <mat-form-field appearance="fill">
          <mat-label>Password</mat-label>
          <input type="password" matInput formControlName="password" />
        </mat-form-field>

        <mat-form-field appearance="fill">
          <mat-label>Password Confirm</mat-label>
          <input type="password" matInput formControlName="passwordConfirm" />
        </mat-form-field>

        <div class="message-container"></div>

        <div class="btn-container">
          <button
            [disabled]="registrationForm.invalid"
            mat-raised-button
            color="primary"
          >
            Signup
          </button>
        </div>
      </form>
    </div>
  `,
  styles: [
    `
      form {
        display: flex;
        flex-direction: column;
        gap: 5px;
      }

      .registration-form-container {
        width: 100%;
        height: 100%;
      }

      :host {
        display: flex;
        justify-content: center;
        align-items: center;
        width: 50%;
        height: 60%;
        margin: 0 auto;
      }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegistrationFormComponet {
  fb = inject(FormBuilder);

  registrationForm = this.fb.group({
    name: ["", Validators.required],
    email: ["", [Validators.required, Validators.email]],
    password: ["", Validators.required],
    passwordConfirm: ["", Validators.required],
  });

  submit() {
    var registrationData: RegistrationModel = Object.assign(
      this.registrationForm.value
    );
    console.log(registrationData);
  }
}
