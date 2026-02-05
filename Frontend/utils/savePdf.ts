import * as DocumentPicker from "expo-document-picker";
import * as FileSystem from "expo-file-system/legacy";

async function savePdfWithPicker(id: number) {
  const url = `${API_BASE_URL}/InvestmentRecommendationPdf/${id}`;

  const tempPath = FileSystem.documentDirectory + `Investment_${id}.pdf`;

  await FileSystem.downloadAsync(url, tempPath);

  const result = await DocumentPicker.getDocumentAsync({
    copyToCacheDirectory: false,
  });

  if (result.assets?.[0]?.uri) {
    await FileSystem.copyAsync({
      from: tempPath,
      to: result.assets[0].uri,
    });
  }
}
