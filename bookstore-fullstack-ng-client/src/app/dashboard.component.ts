import { ChangeDetectionStrategy, Component } from "@angular/core";

@Component({
  selector: "app-dashboard",
  standalone: true,
  imports: [],
  template: "<h2>Welcome</h2>",
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent {}
