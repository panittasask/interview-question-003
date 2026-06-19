export interface menuItem {
  id: string;
  label: string;
  child?: child[];
}
export interface child {
  id: string;
  label: string;
  routes: string;
  visible?: boolean;
}

export const SIDEBAR_MENU: menuItem[] = [
  {
    id: 'doc',
    label: 'อนุมัติ',
    child: [
      {
        id: 'approve_document',
        label: 'เอกสาร',
        routes: 'approve',
        visible: true,
      },
    ],
  },
];
