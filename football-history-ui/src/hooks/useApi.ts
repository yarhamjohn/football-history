const useApi = () => {
  const api = process.env.REACT_APP_FOOTBALL_HISTORY_URL;
  return { api };
};

export { useApi };
