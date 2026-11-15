const styles = {
  Unpaid: 'bg-[#f7dcd6] text-[#9c2b16] ring-[#e3b3a8]',
  Partial: 'bg-[#fbeacb] text-[#7a5210] ring-[#e7cd93]',
  Paid: 'bg-[#dcecdb] text-[#2f6b3a] ring-[#b9d8b6]',
};

export function PaymentBadge({ status }) {
  const tone = styles[status] ?? 'bg-beige-100 text-beige-800 ring-beige-300';

  return (
    <span className={`inline-flex items-center rounded-full px-2.5 py-1 text-xs font-medium ring-1 ring-inset ${tone}`}>
      {status}
    </span>
  );
}
