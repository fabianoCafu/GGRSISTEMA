import React from "react";
import { Column } from "../interface/types";

interface GenericTableProps<T> {
    columns: Column<T>[];
    data: T[];
}

function getCellValue<T>(row: T, column: Column<T>): React.ReactNode {
  if (column.render) {
      return column.render(column.accessor 
          ? (row as any)[column.accessor] 
          : undefined, row);
  }

  if (!column.accessor) {
    return null;
  }

  if (typeof column.accessor === "function") {
    return column.accessor(row);
  }

  const value = row[column.accessor];

  return value !== undefined && value !== null ? String(value) : null;
}

export function GenericTable<T>({ columns, data }: GenericTableProps<T>) {
    return (
        <table className="table table-striped">
            <thead>
                <tr>
                  {columns.map((col, index) => (
                    <th key={index}>
                        {col.header}
                    </th>
                  ))}
                </tr>
            </thead>
        <tbody>
        {data.length === 0 ? (
            <tr>
                <td colSpan={columns.length} style={{ textAlign: "center" }}>
                    Nenhum registro encontrado
                </td>
            </tr>
        ) : (
          data.map((row, rowIndex) => (
            <tr key={rowIndex}>
                {columns.map((col, colIndex) => (
                    <td key={colIndex}>
                        { getCellValue(row, col) }
                    </td>
                ))}
            </tr>
          ))
        )}
      </tbody>
    </table>
  );
}
