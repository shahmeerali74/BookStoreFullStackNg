import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { HeaderComponent } from "./header.component";
import { FooterComponent } from "./footer.component";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent],
  template: `
    <app-header />

    <div class="content">
      <router-outlet />
    </div>
    <app-footer />
  `,
  styles: [
    `
      :host {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
      }
      .content {
        padding: 15px 20px;
      }
    `,
  ],
})
export class AppComponent {
  title = "bookstore-fullstack-ng-client";
}
