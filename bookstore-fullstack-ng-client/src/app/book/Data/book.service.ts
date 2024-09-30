import { inject, Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { PagedList } from "../../common/paged-list.model";
import { BookCreateModel } from "./book-create.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../environments/environment.development";
import { BookReadModel } from "./book-read.model";

@Injectable({ providedIn: "root" })
export class BookService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.baseUrl + "/api/books";

  getBooks({
    pageSize = 10,
    pageNumber = 1,
    searchTerm = "",
    sortBy = "",
    publishedFrom = 0,
    publishedTo = 0,
  }): Observable<PagedList<BookReadModel>> {
    let params = new HttpParams();
    params = params.set("pageSize", pageSize.toString());
    params = params.set("pageNumber", pageNumber.toString());
    params = params.set("searchTerm", searchTerm);
    params = params.set("sortBy", sortBy);
    if (publishedFrom > 0 && publishedTo > 0) {
      params = params.set("publishedFrom", publishedFrom);
      params = params.set("publishedTo", publishedTo);
    }
    return this.http
      .get<PagedList<BookReadModel>>(this.baseUrl, { params })
      .pipe(
        map((response) => ({
          ...response,
          items: response.items.map((book) => ({
            ...book,
            authorNames: book.authors
              .map((author) => author.authorName)
              .join(", ")
              .split(","),
            genreNames: book.genres
              .map((genre) => genre.genreName)
              .join(", ")
              .split(","),
          })),
        }))
      );
  }

  getBookById(id: number): Observable<BookReadModel> {
    return this.http.get<BookReadModel>(this.baseUrl);
  }

  addBook(book: BookCreateModel): Observable<BookReadModel> {
    const formData = new FormData();
    if (book.imageFile) {
      formData.append("imageFile", book.imageFile);
      delete book.imageFile;
    }
    Object.entries(book).forEach(([key, value]) => {
      formData.append(key, value);
    });
    return this.http.post<BookReadModel>(this.baseUrl, formData);
  }

  updateBook(book: BookCreateModel): Observable<any> {
    const formData = new FormData();
    if (book.imageFile) {
      formData.append("imageFile", book.imageFile);
      delete book.imageFile;
    }
    Object.entries(book).forEach(([key, value]) => {
      formData.append(key, value);
    });
    return this.http.put<any>(this.baseUrl + "/" + book.id, formData);
  }

  deleteBook(id: number) {
    return this.http.delete(this.baseUrl + "/" + id);
  }
}
