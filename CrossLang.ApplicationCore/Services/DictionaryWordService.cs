using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.Library;
using CrossLang.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.PronunciationAssessment;

namespace CrossLang.ApplicationCore.Services
{
    public class DictionaryWordService : BaseService<DictionaryWord>, IDictionaryWordService
    {
        public DictionaryWordService(IBaseRepository<DictionaryWord> repository, IHttpContextAccessor httpContextAccessor, SessionData sessionData) : base(repository, httpContextAccessor, sessionData)
        {
        }

        public override ServiceResult GetDetailsById(long id)
        {
            var a = _sessionData.Identity;
            var b = _sessionData.CurrentPrincipal;

            var data = new Dictionary<string, object>();

            var word = this._repository.GetEntityById(id);

            data.Add(nameof(DictionaryWord.ID), word.ID);
            data.Add(nameof(DictionaryWord.Text), word.Text);
            data.Add(nameof(DictionaryWord.Pronunciation), word.Pronunciation);

            var translations = this._repository.GetDetailsById(id);

            var translationsConverted = translations.GroupBy(u =>
            {
                u.TryGetValue("language", out object? value);
                return value?.ToString() ?? "";
            })
                .Select(grp => grp.ToList())
                .ToDictionary(x => x[0]["language"] ?? "", x => x);

            data.Add("Translations", translationsConverted);

            serviceResult.SuccessState = true;
            serviceResult.Data = data;
            return serviceResult;
        }

        public ServiceResult GetSearchResult(string text)
        {
            serviceResult.SuccessState = true;
            serviceResult.Data = (this._repository as IDictionaryWordRepository).GetSearchResult(text);
            return serviceResult;
        }

        public ServiceResult PronunciationAssess(object input)
        {
            return serviceResult;
        }

        // Pronunciation assessment with microphone as audio input.
        // See more information at https://aka.ms/csspeech/pa
        public async Task<ServiceResult> PronunciationAssessmentAsync(string fileName, string word)
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("e8650ddda68d40dca1bc6268aa0e64eb", "southeastasia");

            // Replace the language with your language in BCP-47 format, e.g., en-US.
            var language = "en-US";

            // The pronunciation assessment service has a longer default end silence timeout (5 seconds) than normal STT
            // as the pronunciation assessment is widely used in education scenario where kids have longer break in reading.
            // You can adjust the end silence timeout based on your real scenario.
            config.SetProperty(PropertyId.SpeechServiceConnection_EndSilenceTimeoutMs, "3000");

            var referenceText = word;
            // create pronunciation assessment config, set grading system, granularity and if enable miscue based on your requirement.
            var pronunciationConfig = new PronunciationAssessmentConfig(referenceText,
                GradingSystem.HundredMark, Granularity.Phoneme, true);

            using (var audioInput = AudioConfig.FromWavFileInput(@$"/Users/mhoang/Documents/Projects/CrossLang/cross-lang-server/CrossLang.API/Temp/{fileName}"))
            {
                // Creates a speech recognizer for the specified language, using microphone as audio input.
                using (var recognizer = new SpeechRecognizer(config, language, audioInput))
                {
                    while (true)
                    {
                        pronunciationConfig.ReferenceText = referenceText;

                        // Starts recognizing.
                        pronunciationConfig.ApplyTo(recognizer);

                        // Starts speech recognition, and returns after a single utterance is recognized.
                        // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
                        var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                        var pronunciationResult = PronunciationAssessmentResult.FromResult(result);
                      

                        serviceResult.SuccessState = true;
                        serviceResult.Data = pronunciationResult;
                        return serviceResult;
                    }
                }
            }

            return serviceResult;
        }
    }
}
