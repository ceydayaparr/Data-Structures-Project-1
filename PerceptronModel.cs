using System;
using System.Collections.Generic;

public class Neuron
{
    private double[] weights;
    private double bias;
    private double learningRate;

    // Constructor
    public Neuron(int inputSize, double learningRate)
    {
        // Başlangıçta ağırlıkları [0, 1] arasında rastgele pozitif double değerlerle başlatır
        Random random = new Random();
        weights = new double[inputSize];
        for (int i = 0; i < inputSize; i++)
        {
            weights[i] = random.NextDouble();
        }

        // Bias'i de [0, 1] arasında rastgele pozitif double değerle başlatır
        bias = random.NextDouble();

        // Öğrenme katsayısını ayarlar
        this.learningRate = learningRate;
    }

    // Çıktıyı hesaplar
    public double CalculateOutput(double[] inputs)
    {
        // Girişlerle ağırlıkların iç çarpımını alır ve bias ekleyerek toplar
        double sum = bias;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i] * inputs[i];
        }

        // Sigmoid aktivasyon fonksiyonunu uygular
        return Sigmoid(sum);
    }

    // Ağırlıkları günceller
    public void UpdateWeights(double[] inputs, double target)
    {
        double output = CalculateOutput(inputs);
        double error = target - output;

        // Ağırlıkları günceller
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] += learningRate * error * inputs[i];
        }

        // Bias'i günceller
        bias += learningRate * error;
    }

    // Sigmoid aktivasyon fonksiyonu
    private double Sigmoid(double x)
    {
        return 1.0 / (1.0 + Math.Exp(-x));
    }

    // Yeni veriler üzerinde tahmin yapar
    public void PredictAndPrint(List<double[]> newTestData)
    {
        Console.WriteLine("\nYeni Veriler ve Tahmin Sonuçları:");
        foreach (var entry in newTestData)
        {
            double[] inputs = new double[] { entry[0], entry[1] };
            double target = entry[2];

            double output = CalculateOutput(inputs);

            Console.WriteLine($"Giriş: {entry[0]}, {entry[1]} | Hedef Çıktı: {target} | Tahmin Çıktı: {output}");
        }
    }
}

class Program
{
    static void Main()
    {
        // Eğitim verileri
        List<double[]> trainingData = new List<double[]>
        {
            new double[] { 7.6 / 10, 11.0 / 15, 77.0 / 100 }, 
            new double[] { 8.0 / 10, 10.0 / 15, 70.0 / 100 },
            new double[] { 6.6 / 10, 8.0 / 15, 55.0 / 100 },
            new double[] { 8.4 /10,10.0 / 15,78.0 / 100 },
            new double[] { 8.80 / 10 ,12.0 / 15,95.0 / 100 },
            new double[] { 7.2 / 10,10.0 / 15,67.0 / 100 },
            new double[] { 8.1 / 10,11.0 / 15 ,80.0 / 100},
            new double[] { 9.5 / 10,9.0 / 15,87.0 / 100},
            new double[] { 7.3 / 10,9.0 / 15,60.0 / 100},
            new double[] { 8.9 / 10,11.0 / 15,88.0 / 100},
            new double[] { 7.5 / 10,11.0 / 15, 72.0 / 100},
            new double[] { 7.6 / 10, 9.0 / 15, 58.0 / 100},
            new double[] { 7.9 / 10,10.0 / 15,70.0 / 100 },
            new double[] { 8.0 / 10,10.0 / 15,76.0 / 100 },
            new double[] { 7.2 / 10,9.0 / 15,58.0 / 100 },
            new double[] { 8.8 / 10,10.0 / 15,81.0 / 100 },
            new double[] { 7.6 / 10, 11.0 / 15, 74.0 / 100 },
            new double[] { 7.5 / 10,10.0 / 15,67.0 / 100 },
            new double[] { 9.0 / 10,10.0 / 15,82.0 / 100 },
            new double[] { 7.7 / 10,9.0 / 15,62.0 / 100 },
            new double[] { 8.1 / 10,11.0 / 15,82.0 / 100 },

           
        };

        // Yeni veriler
        List<double[]> newTestData = new List<double[]>
        {
            new double[] { 8.2 / 10, 12.0 / 15, 0 },  // Sınav Sonucu belirtilmemiş, 0 olarak geçici olarak atandı
            new double[] { 6.5 / 10, 9.0 / 15, 0 },
            new double[] { 9.0 / 10, 13.0 / 15, 0 },
            new double[] { 7.0 / 10, 10.0 / 15, 0 },
            new double[] { 8.5 / 10, 11.0 / 15, 0 }
        };

        // Giriş boyutu
        int inputSize = 2; // Çalışma Süresi ve Derse Devam

        // Eğitim sonrası tahminler için kullanılacak parametreler
        int[] epochsList = { 10, 50, 100 };
        double[] learningRatesList = { 0.01, 0.025, 0.05 };

        // Eğitim sonrası tahminler ve MSE değerlerini hesaplar ve yazdırır
        CalculateAndPrintMSEValues(trainingData, newTestData, inputSize, epochsList, learningRatesList);
    }

    // Eğitim sonrası tahminler ve MSE değerlerini hesaplar ve yazdırır
    static void CalculateAndPrintMSEValues(List<double[]> trainingData, List<double[]> newTestData, int inputSize, int[] epochsList, double[] learningRatesList)
    {
        Console.WriteLine("Eğitim Sonrası Tahminler ve MSE Değerleri:");

        foreach (var epochs in epochsList)
        {
            foreach (var learningRate in learningRatesList)
            {
                // Yapay sinir hücresi oluşturur
                Neuron neuron = new Neuron(inputSize, learningRate);
                // Eğitim işlemi
                for (int epoch = 0; epoch < epochs; epoch++)
                {
                    foreach (var entry in trainingData)
                    {
                        double[] inputs = new double[] { entry[0], entry[1] };
                        double target = entry[2];

                        // Ağı eğit ve ağırlıkları günceller
                        neuron.UpdateWeights(inputs, target);
                    }
                }

                // Eğitim sonrası tahminler
                Console.WriteLine($"\nEpochs: {epochs}, Learning Rate: {learningRate}");
                foreach (var entry in trainingData)
                {
                    double[] inputs = new double[] { entry[0], entry[1] };
                    double target = entry[2];

                    double output = neuron.CalculateOutput(inputs);

                    Console.WriteLine($"Giriş: {entry[0]}, {entry[1]} | Hedef Çıktı: {target} | Tahmin Çıktı: {output}");
                }

                // MSE Hesaplama
                double mse = CalculateMeanSquareError(neuron, trainingData);

                Console.WriteLine($"MSE (Mean Square Error): {mse}");

                // Yeni veriler üzerinde tahmin yapar
                neuron.PredictAndPrint(newTestData);
            }
        }
    }

    // MSE Hesaplama
    static double CalculateMeanSquareError(Neuron neuron, List<double[]> data)
    {
        double sumSquaredErrors = 0;

        foreach (var entry in data)
        {
            double[] inputs = new double[] { entry[0], entry[1] };
            double target = entry[2];

            double output = neuron.CalculateOutput(inputs);

            double error = target - output;
            sumSquaredErrors += error * error;
        }

        return sumSquaredErrors / data.Count;
    }
}