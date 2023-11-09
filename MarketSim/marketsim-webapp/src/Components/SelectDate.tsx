
import { useState } from "react";

function SelectDate() {
  const [selectedDate, setSelectedDate] = useState<any>();

  const submit = async () => {
    const requestOptions = {
      method: 'POST',
      headers: { 'Accept': 'application/json',
                'Content-Type': 'application/json' },
      body: `\"${selectedDate}\"`
    };

    const response = await fetch(`http://localhost:5234/Seed/SystemSettings`, requestOptions);
  }

  return (<>
    <input
      type="date"
      id="datePicker"
      value={selectedDate}
      onChange={(e) => setSelectedDate(e.target.value)}
    />
    <button onClick={submit}>Submit</button>
  </>)
}

export default SelectDate;