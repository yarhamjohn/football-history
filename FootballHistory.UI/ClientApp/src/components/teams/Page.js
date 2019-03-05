import React, { useState } from 'react';

function Page() {
  const [selectedTeam, setSelectedTeam] = useState('Test Team');

  return (
      <div>
        <h1>{selectedTeam}</h1>
      </div>
  );
}

export default Page;
