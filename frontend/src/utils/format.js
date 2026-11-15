const peso = new Intl.NumberFormat('en-PH', {
  style: 'currency',
  currency: 'PHP',
  minimumFractionDigits: 2,
});

const wholeNumber = new Intl.NumberFormat('en-PH');

const dateAndTime = new Intl.DateTimeFormat('en-PH', {
  month: 'short',
  day: 'numeric',
  year: 'numeric',
  hour: 'numeric',
  minute: '2-digit',
  hour12: true,
  timeZone: 'Asia/Manila',
});

export function formatPeso(value) {
  return peso.format(Number(value ?? 0));
}

export function formatCount(value) {
  return wholeNumber.format(Number(value ?? 0));
}

export function formatDate(value) {
  if (!value) return '—';
  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? '—' : dateAndTime.format(parsed);
}

export function formatPercent(part, total) {
  if (!total) return '0%';
  return `${Math.round((part / total) * 100)}%`;
}
