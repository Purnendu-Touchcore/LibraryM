export interface Record {
  id: number
  bookId: number
  bookName: string
  userId: number
  email: string
  status: string
  issuedDate: Date | null
  dueDate: Date | null
  returnDate: Date | null
  fineAmount: number
  maxRows: number
}