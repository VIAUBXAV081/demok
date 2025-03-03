import { useState } from "react";
import { TranslationLanguage } from "../services/TranslationService";

function TranslationSelector(props: {onLanguageChange : (lang: TranslationLanguage|null) => void}) {

    const languages = Object.entries(TranslationLanguage).map(lang => ({ key: lang[1], value: lang[0] }));

    const [selectedLanguage, setSelectedLanguage] = useState<TranslationLanguage|null>();

    function handleLanguageChange(lang: TranslationLanguage | null) {
        setSelectedLanguage(lang);
        props.onLanguageChange(lang);
    }

    const resetLanguage = () => handleLanguageChange(null);

    return (
        <div className="bg-body-secondary px-4 py-2 rounded mt-4">
            <h4>Translate this post</h4>
            <ul>
                {languages.map((lang) => (

                    <li key={lang.key}>
                        <a role="button" className={`text-decoration-none ${selectedLanguage === lang.key ? "fw-bold" : ""}`} onClick={() => { handleLanguageChange(lang.key) }}>{lang.value}</a>
                        {(selectedLanguage !== null && selectedLanguage === lang.key) ?
                            <a role="button" className="text-decoration-none ms-2" onClick={resetLanguage}>
                                <i className="bi bi-x-circle"></i>
                            </a> : ""
                        }
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default TranslationSelector;