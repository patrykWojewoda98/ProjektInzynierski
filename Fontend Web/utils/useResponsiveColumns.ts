import { useWindowDimensions } from "react-native";

export const useResponsiveColumns = (maxColumns = 4) => {
  const { width } = useWindowDimensions();

  const columns =
    width >= 1400
      ? Math.min(maxColumns, 4)
      : width >= 1100
        ? Math.min(maxColumns, 3)
        : width >= 700
          ? Math.min(maxColumns, 2)
          : 1;

  const itemWidth = `${100 / columns - 4}%`;

  return { columns, itemWidth };
};
