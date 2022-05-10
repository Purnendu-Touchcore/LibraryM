export interface Record {
  id: number
  bookId: number
  bookName: string
  userId: number,
  email: string
  status: string
  issuedDate: string
  dueDate: string
  returnDate: string
  fineAmount: number
  maxRows: number
}