import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from "@angular/core";
import { Author } from "../data/author.model";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: "app-author-list",
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule],
  template: `
    <table mat-table [dataSource]="authors" class="mat-elevation-z8">
      <ng-container matColumnDef="authorName">
        <th mat-header-cell *matHeaderCellDef>Author</th>
        <td mat-cell *matCellDef="let element">{{ element.authorName }}</td>
      </ng-container>

      <ng-container matColumnDef="action">
        <th mat-header-cell *matHeaderCellDef>Action</th>
        <td
          mat-cell
          *matCellDef="let author"
          style="display: flex;gap:5px;padding:5px;align-items:center;"
        >
          <button
            type="button"
            (click)="edit.emit(author)"
            mat-mini-fab
            color="secondary"
          >
            <mat-icon>edit</mat-icon>
          </button>
          <button
            type="button"
            (click)="delete.emit(author)"
            mat-mini-fab
            color="warn"
          >
            <mat-icon>delete</mat-icon>
          </button>
        </td>
      </ng-container>
      <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
      <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
    </table>
  `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthorListComponent {
  @Input({ required: true }) authors!: Author[];
  @Output() edit = new EventEmitter<Author>();
  @Output() delete = new EventEmitter<Author>();

  displayedColumns = ["AuthorName,Action"];
}
