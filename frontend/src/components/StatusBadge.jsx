const styles = {
  Pending: 'bg-[#fbeacb] text-[#7a5210] ring-[#e7cd93]',
  Confirmed: 'bg-[#dcecdb] text-[#2f6b3a] ring-[#b9d8b6]',
  CheckedIn: 'bg-[#d9e7f3] text-[#1f4f7a] ring-[#b4cee6]',
  Cancelled: 'bg-[#f3dedb] text-[#8a3232] ring-[#e2bbb5]',
};

const labels = {
  Pending: 'Pending',
  Confirmed: 'Confirmed',
  CheckedIn: 'Checked In',
  Cancelled: 'Cancelled',
};

export function StatusBadge({ status }) {
  const tone = styles[status] ?? 'bg-beige-100 text-beige-800 ring-beige-300';

  return (
    <span className={`inline-flex items-center rounded-full px-2.5 py-1 text-xs font-medium ring-1 ring-inset ${tone}`}>
      {labels[status] ?? status}
    </span>
  );
}
