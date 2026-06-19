import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface ApproveTableItem {
  id: number;
  item: string;
  reason: string;
  status: string;
  statusCode: number;
}

interface DocumentApiItem {
  id: number;
  documentName: string;
  documentDetail: string;
  status: number;
  statusReason: string | null;
}

interface UpdateDocumentStatusRequest {
  id: number;
  status: number;
  description: string;
}

@Component({
  selector: 'app-approve-document',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './approve-document.component.html',
  styleUrl: './approve-document.component.scss',
})
export class ApproveDocumentComponent {
  private readonly http = inject(HttpClient);

  rows: ApproveTableItem[] = [];

  selectedIds = new Set<number>();
  modalAction: 'อนุมัติ' | 'ไม่อนุมัติ' | null = null;
  reasonText = '';
  isLoading = false;

  get allSelected(): boolean {
    return (
      this.selectableRows.length > 0 &&
      this.selectedIds.size === this.selectableRows.length
    );
  }

  get selectedCount(): number {
    return this.selectedIds.size;
  }

  get selectableRows(): ApproveTableItem[] {
    return this.rows.filter((row) => row.statusCode === 1);
  }

  constructor() {
    this.loadDocuments();
  }

  isSelected(id: number): boolean {
    return this.selectedIds.has(id);
  }

  onToggleAll(event: Event): void {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      this.selectedIds = new Set(this.selectableRows.map((row) => row.id));
      return;
    }
    this.selectedIds.clear();
  }

  onToggleRowChange(id: number, event: Event): void {
    if (!this.isRowSelectable(id)) {
      return;
    }

    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      this.selectedIds.add(id);
      return;
    }
    this.selectedIds.delete(id);
  }

  toggleRowByClick(id: number): void {
    if (!this.isRowSelectable(id)) {
      return;
    }

    if (this.selectedIds.has(id)) {
      this.selectedIds.delete(id);
      return;
    }
    this.selectedIds.add(id);
  }

  setModalAction(action: 'อนุมัติ' | 'ไม่อนุมัติ'): void {
    if (this.selectedIds.size === 0 || this.isLoading) {
      return;
    }

    this.modalAction = action;
  }

  confirmAction(modalAction = this.modalAction): void {
    if (!modalAction || this.selectedIds.size === 0) {
      this.modalAction = null;
      return;
    }

    const status = modalAction === 'อนุมัติ' ? 2 : 3;
    const payloads: UpdateDocumentStatusRequest[] = [...this.selectedIds].map(
      (id) => ({
        id,
        status,
        description: this.reasonText,
      }),
    );

    this.isLoading = true;

    Promise.all(payloads.map((payload) => this.updateStatus(payload)))
      .then(() => {
        this.selectedIds.clear();
        this.reasonText = '';
        this.loadDocuments();
      })
      .finally(() => {
        this.isLoading = false;
        this.modalAction = null;
      });
  }

  private loadDocuments(): void {
    this.http.get<DocumentApiItem[]>('/api/documents').subscribe({
      next: (items) => {
        this.rows = items.map((item) => ({
          id: item.id,
          item: item.documentName,
          reason: item.documentDetail,
          status: this.toStatusText(item.status),
          statusCode: item.status,
        }));
        const availableIds = new Set(
          this.rows.filter((x) => x.statusCode === 1).map((x) => x.id),
        );
        this.selectedIds.forEach((id) => {
          if (!availableIds.has(id)) {
            this.selectedIds.delete(id);
          }
        });
      },
      error: () => {
        this.rows = [];
        this.selectedIds.clear();
      },
    });
  }

  private updateStatus(payload: UpdateDocumentStatusRequest): Promise<void> {
    return new Promise((resolve) => {
      this.http.post('/api/documents/status', payload).subscribe({
        next: () => resolve(),
        error: () => resolve(),
      });
    });
  }

  private isRowSelectable(id: number): boolean {
    const row = this.rows.find((x) => x.id === id);
    return !!row && row.statusCode === 1;
  }

  private toStatusText(status: number): string {
    if (status === 1) {
      return 'รออนุมัติ';
    }

    if (status === 2) {
      return 'อนุมัติแล้ว';
    }

    if (status === 3) {
      return 'ไม่อนุมัติ';
    }

    return 'ไม่ทราบสถานะ';
  }
}
