# Car Show 2027 — Registration Dashboard (React)

Vite + React + Tailwind CSS dashboard for the Car Show Registration System.

## Scripts

| Command | Purpose |
| --- | --- |
| `npm install` | Install dependencies |
| `npm run dev` | Start the dev server on http://localhost:5173 |
| `npm run build` | Production build into `dist/` |
| `npm run preview` | Preview the production build |

## Environment

`frontend/.env.development`:

```
VITE_API_BASE_URL=http://localhost:5205
VITE_DATA_SOURCE=api
```

`VITE_API_BASE_URL` is the backend address used by every fetch call.

## Switching between sample data and the real API

The dashboard reads through `src/api/index.js`, which exports `getRegistrations` and `getSummary`.
Both implementations return the same JSON shapes, so no component changes are needed.

- **Real API:** set `VITE_DATA_SOURCE=api` (default) in `.env.development`. Requests go to
  `src/api/registrationsApi.js` and reach `http://localhost:5205/api/registrations`.
- **Sample data:** set `VITE_DATA_SOURCE=sample`, then restart `npm run dev`. Requests go to
  `src/api/sample-data.js`, which holds 25 registrations and filters, pages and totals them in the browser.

The header shows a `Live API` or `Sample data` pill so the current source is always visible.

## Folder map

```
src/
├── api/
│   ├── index.js              picks the data source
│   ├── registrationsApi.js   fetch calls to the C# API
│   └── sample-data.js        25 local registrations
├── components/
│   ├── KpiCard.jsx           one card and its skeleton
│   ├── KpiCards.jsx          the row of six cards
│   ├── RegistrationFilters.jsx
│   ├── RegistrationTable.jsx table, skeleton rows
│   ├── Pagination.jsx
│   ├── StatusBadge.jsx
│   └── PaymentBadge.jsx
├── hooks/
│   ├── useDebouncedValue.js
│   └── useDashboardData.js
├── pages/
│   └── RegistrationsPage.jsx
├── utils/format.js
├── App.jsx
├── index.css
└── main.jsx
```

## Screens

- KPI cards: total registrations, confirmed, checked in, total payments, uncollected, collection rate.
- Filters: participant search, registration type, status, clear filters.
- Table: registration number, date, participant, car, type, status, fee, paid, payment badge.
- Pagination: prev/next, page numbers, page-size selector, `Showing x–y of N`.
- States: loading message with skeleton rows, empty state with clear filters, error state with try again.
