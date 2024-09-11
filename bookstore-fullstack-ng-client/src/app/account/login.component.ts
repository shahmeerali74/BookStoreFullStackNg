import { ChangeDetectionStrategy, Component } from "@angular/core";
import { LoginFormComponet } from "./ui/login-form.component";
import { LoginModel } from "./data/login.model";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [LoginFormComponet],
  template: `
    <div class="container">
      <h2>Login</h2>
      <div style="margin-bottom: 8px;"></div>
      <app-login-form (submit)="onSubmit($event)" />
    </div>
  `,
  styles: `
  .container{
    width:40%;
    display:flex;
    flex-direction:column;
    margin: auto;
    border:1px solid grey;
    padding:10px;
  }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  onSubmit(data: LoginModel) {
    console.log(data);
  }
}
