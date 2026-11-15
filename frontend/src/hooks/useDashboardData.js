import { useCallback, useEffect, useState } from 'react';
import { getRegistrations, getSummary } from '../api';

const emptyState = {
  registrations: null,
  summary: null,
  loading: true,
  error: null,
};

export function useDashboardData(filters) {
  const [state, setState] = useState(emptyState);
  const [attempt, setAttempt] = useState(0);

  const { participantName, type, status, page, pageSize } = filters;

  useEffect(() => {
    const controller = new AbortController();
    let active = true;

    setState((current) => ({ ...current, loading: true, error: null }));

    Promise.all([
      getRegistrations({ participantName, type, status, page, pageSize }, controller.signal),
      getSummary(controller.signal),
    ])
      .then(([registrations, summary]) => {
        if (!active) return;
        setState({ registrations, summary, loading: false, error: null });
      })
      .catch((error) => {
        if (!active || error.name === 'AbortError') return;
        setState({ registrations: null, summary: null, loading: false, error: error.message });
      });

    return () => {
      active = false;
      controller.abort();
    };
  }, [participantName, type, status, page, pageSize, attempt]);

  const retry = useCallback(() => setAttempt((value) => value + 1), []);

  return { ...state, retry };
}
