using System.Collections.Generic;

namespace Swift_Solitaire.Scripts.UI
{
    public static class ShuffleTool
    {
        public static int[] ArrangeArray(int[] arr, int x, int y, bool hasTruePos) {
            int n = arr.Length;
            int[] result = new int[x * y];

            // Khởi tạo mảng kết quả với -1
            for (int i = 0; i < x * y; i++) {
                result[i] = -1;
            }

            // Tạo danh sách các số có sẵn từ mảng đầu vào
            List<int> availableNumbers = new List<int>(arr);
            System.Random random = new System.Random();

            // Nếu hasTruePos = true, đặt các số đúng vị trí và loại bỏ khỏi availableNumbers
            if (hasTruePos) {
                for (int i = 0; i < n; i++) {
                    if (arr[i] < x * y && arr[i] >= 0 && arr[i] == i) {
                        result[i] = arr[i];
                        availableNumbers.Remove(arr[i]);
                    }
                }
            }

            // Sắp xếp ngẫu nhiên các số còn lại
            for (int i = 0; i < x * y && availableNumbers.Count > 0; i++) {
                if (result[i] == -1) {
                    // Tạo danh sách các số thỏa điều kiện (ưu tiên result[i] != i và result[i] != arr[i])
                    List<int> validNumbers = new List<int>();
                    for (int j = 0; j < availableNumbers.Count; j++) {
                        if (availableNumbers[j] != i && (i >= n || availableNumbers[j] != arr[i])) {
                            validNumbers.Add(availableNumbers[j]);
                        }
                    }

                    int selectedNumber;
                    if (validNumbers.Count > 0) {
                        // Chọn ngẫu nhiên một số từ validNumbers
                        int randomIndex = random.Next(0, validNumbers.Count);
                        selectedNumber = validNumbers[randomIndex];
                    }
                    else {
                        // Nếu không có số thỏa cả hai điều kiện, dùng số bất kỳ còn lại để tránh -1
                        int randomIndex = random.Next(0, availableNumbers.Count);
                        selectedNumber = availableNumbers[randomIndex];
                    }

                    result[i] = selectedNumber;
                    availableNumbers.Remove(selectedNumber);
                }
            }

            return result;
        }
    }
}
