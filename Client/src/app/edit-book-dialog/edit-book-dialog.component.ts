import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Book } from '../book/book';
import { BookService } from '../book/book.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-book-dialog',
  templateUrl: './edit-book-dialog.component.html',
  styleUrls: ['./edit-book-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [BookService],
})
export class EditBookDialogComponent implements OnInit {


  form: FormGroup;

  dialogTitle: string;
  coverImage: any = '';
  book: Book;
  prevTotalQnt: number;
  publicationDate: string;
  mode: 'create' | 'update';

  constructor(private fb: FormBuilder,
    private dialogRef: MatDialogRef<EditBookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    private bookService: BookService,
    private toastr: ToastrService
    ) {

    this.dialogTitle = data.dialogTitle;
    this.book = data.book;
    this.mode = data.mode;

    const formControls = {
      name: ['', Validators.required],
      totalQuantity: [0, Validators.required],
      author: ['', Validators.required],
      description: ['', Validators.required],
      publisher: ['', Validators.required],
      isbn: [0, Validators.required],
      publicationDate: ['', Validators.required],
      pages: [0, Validators.required],
    };

    this.form = this.fb.group(formControls);

    if (this.mode == 'update') {
      this.form.patchValue({ ...data.book });
      this.prevTotalQnt = data.book.totalQuantity
      this.form.controls['publicationDate'].setValue(new Date(this.form.value.publicationDate).toISOString().split('T')[0]);
      this.coverImage = data.book.coverImage;
    }

  }

  ngOnInit(): void {
  }

  onClose() {
    this.dialogRef.close();
  }

  handleInputChange(e: any) {

    if (e.target.files[0]) {
      let file = e.target.files[0]
      let contentType = e.target.name
      let reader = new FileReader();
      reader.onload = (e) => {
        this.coverImage = e.target?.result;
      };
      reader.readAsDataURL(file);
    }
  }

  onSave() {

    const book: Book = {
      ...this.book,
      ...this.form.value
    };

    book.coverImage = this.coverImage;
    if (this.mode == 'update') {

      book.availableQuantity += book.totalQuantity - this.prevTotalQnt
      if (book.availableQuantity < 0) {
        book.availableQuantity = 0
      }
      if (book.totalQuantity < 0) {
        book.totalQuantity = 0
      }
      this.bookService.updateBook(book)
      .subscribe(res => this.toastr.success(res.message));

      this.dialogRef.close();
    } else if (this.mode == 'create') {

      book.availableQuantity = book.totalQuantity;
      console.log(book);
      this.bookService.addBook(book)
        .subscribe(
          newBook => {
            this.toastr.success("Book added Successfully");
            this.dialogRef.close();

          }
        );
    }
  }

}
