import { Component } from '@angular/core';

interface ApproveTableItem {
  id: number;
  item: string;
  reason: string;
  status: string;
}

@Component({
  selector: 'app-approve-document',
  standalone: true,
  imports: [],
  templateUrl: './approve-document.component.html',
  styleUrl: './approve-document.component.scss',
})
export class ApproveDocumentComponent {
  rows: ApproveTableItem[] = [
    {
      id: 1,
      item: 'ใบขอซื้อ PR-2026-001',
      reason: 'จัดซื้ออุปกรณ์สำนักงาน',
      status: 'รออนุมัติ',
    },
    {
      id: 2,
      item: 'ใบเบิกค่าเดินทาง TR-2026-014',
      reason: 'เดินทางเข้าพบลูกค้า',
      status: 'รอเอกสารเพิ่ม',
    },
    {
      id: 3,
      item: 'ใบสั่งซ่อม MT-2026-008',
      reason: 'บำรุงรักษาเครื่องจักร',
      status: 'อนุมัติแล้ว',
    },
  ];
}
